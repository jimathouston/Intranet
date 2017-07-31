import { Component, OnInit } from '@angular/core'
import * as _ from 'lodash'
import { Observable } from 'rxjs/Observable'
import {
  Category,
  Faq,
  FaqByCategory
} from '../../models'
import {
  AuthenticationService,
  CategoryService,
  FaqService,
} from '../../_services'

// See https://github.com/lodash/lodash/issues/1677#issuecomment-306119559
const toggler = (collection, item) => {
    const idx = _.indexOf(collection, item)
    if (idx !== -1) {
        collection.splice(idx, 1)
    } else {
        collection.push(item)
    }
}

@Component({
  selector: 'faq',
  templateUrl: './faq.component.html',
  styleUrls: ['./faq.component.css']

})
export class FaqComponent implements OnInit {
  faqByCategories: FaqByCategory[]
  openFaqs: number[]
  existingFaqIds: number[]
  newFaqIds: number[]
  newCategory: Category
  isAdmin: boolean
  saved: {
    categoryId: number,
    faqId: number,
    success: string | null,
    error: string | null,
  }

  constructor(
    private authenticationService: AuthenticationService,
    private categoryService: CategoryService,
    private faqService: FaqService
  ) {
    this.openFaqs = []
    this.saved = {
      categoryId: null,
      faqId: null,
      success: null,
      error: null,
    }
    this.existingFaqIds = []
    this.newFaqIds = []
    this.newCategory = new Category()
    this.newCategory.id = 0
    this.newCategory.title = 'Add a new category'
  }

  async ngOnInit() {
    this.updateData()
    this.isAdmin = await this.authenticationService.isAdmin()
  }

  onCategoryTitleChange(content: string, category: Category) {
    category.title = content
  }

  onQuestionChange(content: string, faq: Faq) {
    faq.question = content
  }

  onAnswerChange(content: string, faq: Faq) {
    faq.answer = content
  }

  onKeywordsChange(content: string, faq: Faq) {
    faq.keywords = content
  }

  handleOnDelete(info: string) {
    this.updateData()
  }

  toggle(id: number) {
    _.partial(toggler, this.openFaqs)(id)
  }

  isOpen(id: number) {
    return _.includes(this.openFaqs, id)
  }

  updateData() {
    Observable
      .zip(
        this.faqService.getFaqsByCategory(),
        this.categoryService.getItems(),
      )
      .subscribe(
        data => {
          const faqByCategories = data[0]
          const categories = data[1].filter(c => c.faqs.length === 0)

          if (this.isAdmin) {
            // Add all empty categories
            for (const category of categories) {
              const newFaqByCategory = new FaqByCategory()
              newFaqByCategory.category = category
              newFaqByCategory.faqs = []
              faqByCategories.push(newFaqByCategory)
            }

            // Get all existing id's (needed to create uniq id's for TinyMCE)
            _.chain(faqByCategories)
              .flatMap(fbc => fbc.faqs)
              .forEach(f => this.existingFaqIds.push(f.id))
              .value()

            // Add a new Faq for each category
            for (const faqByCategory of faqByCategories) {
              const newFaq = new Faq()
              newFaq.category = new Category()
              newFaq.answer = 'Enter a new answer.'
              newFaq.question = 'Enter a new question.'
              newFaq.keywords = 'Add Keywords!'
              newFaq.category.title = faqByCategory.category.title

              // Add a uniq id to each all new faq's
              const id = (_.chain(_.concat(this.existingFaqIds, this.newFaqIds)) as any)
                .max()
                .value() + 100

              newFaq.id = id
              this.newFaqIds.push(newFaq.id)

              faqByCategory.faqs.push(newFaq)
            }
          }

          this.faqByCategories = faqByCategories
        }
      )
  }

  async save(faq: Faq) {
    faq.category.faqs = null
    faq.faqKeywords = null

    let obs: Observable<Faq>

    if (_.includes(this.existingFaqIds, faq.id)) {
      obs = await this.faqService.putItem(faq)
    } else {
      faq.id = 0
      obs = await this.faqService.postItem(faq)
    }

    obs.subscribe(
      (data) => {
        faq.id = data.id
        this.saved.categoryId = null
        this.saved.faqId = data.id
        this.saved.success = faq.id === 0 ? 'Created successfully!' : 'Updated successfully!'
        this.saved.error = null
        this.updateData()
      },
      error => {
        this.saved.categoryId = null
        this.saved.faqId = faq.id
        this.saved.success = null
        this.saved.error = error
      }
    )
  }

  async delete(faq: Faq) {
    await this.faqService.deleteItem(faq.id).subscribe(
      (data) => {
        this.saved.categoryId = null
        this.saved.faqId = faq.id
        this.saved.success = 'Deleted successfully!'
        this.saved.error = null
      },
      error => {
        this.saved.categoryId = null
        this.saved.faqId = faq.id
        this.saved.success = null
        this.saved.error = error
      }
    )
  }

    async saveCategory(originalCategory: Category) {
    let obs: Observable<Category>
    const category = new Category()
    category.id = originalCategory.id
    category.title = originalCategory.title

    if (category.id && category.id !== 0) {
      obs = await this.categoryService.putItem(category)
    } else {
      obs = await this.categoryService.postItem(category)
    }

    obs.subscribe(
      (data) => {
        category.id = data.id
        this.saved.categoryId = data.id
        this.saved.faqId = null
        this.saved.success = category.id === 0 || category.id === null ? 'Created successfully!' : 'Updated successfully!'
        this.saved.error = null
        this.updateData()
      },
      error => {
        console.log(error)
        this.saved.categoryId = category.id
        this.saved.faqId = null
        this.saved.success = null
        this.saved.error = error
      }
    )
  }

  async deleteCategory(category: Category) {
    await this.categoryService.deleteItem(category.id).subscribe(
      (data) => {
        this.saved.categoryId = category.id
        this.saved.faqId = null
        this.saved.success = 'Deleted successfully!'
        this.saved.error = null
      },
      error => {
        this.saved.categoryId = category.id
        this.saved.faqId = null
        this.saved.success = null
        this.saved.error = error
      }
    )
  }
}
