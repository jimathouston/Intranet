import * as React from 'react'
import { Link } from 'react-router'

export class NavMenu extends React.Component<void, void> {
    public render() {
        return <div className='main-nav'>
                    <nav className='navbar navbar-inverse navbar-fixed-top'>
                           <div className='container-fluid'>
                                   <div className='navbar-header'>
                                           <button type='button' className='navbar-toggle collapsed' data-toggle='collapse'
                                           data-target='#navbar' aria-expanded='false' aria-controls='navbar'>
                                               <span className='sr-only'>Toggle navigation</span>
                                               <span className='icon-bar'></span>
                                               <span className='icon-bar'></span>
                                               <span className='icon-bar'></span>
                                           </button>
                                           <a className='navbar-brand' href={ '/' }>Certaincy</a>
                                    </div>
                                       <div id='navbar' className='navbar-collapse collapse'>
                                           <ul className='nav navbar-nav navbar-right'>
                                               <li><a href='#'>Dashboard</a></li>
                                               <li><a href='#'>Settings</a></li>
                                               <li><a href='#'>Profile</a></li>
                                               <li><a href='#'>Help</a></li>
                                           </ul>
                                           <form className='navbar-form navbar-right'>
                                               <input type='text' className='form-control' placeholder='Search...'/>
                                           </form>
                                   </div>
                           </div>
                    </nav>

                    <div className='container-fluid'>
                        <div className='row'>
                            <div className='col-sm-3 col-md-2 sidebar'>
                                <ul className='nav nav-sidebar'>
                                    <li className='active'><a href='#'>Overview <span className='sr-only'>(current)</span></a></li>
                                    <li>
                                        <Link to={ '/' } activeClassName='active'>
                                            <span className='glyphicon glyphicon-home'></span> News
                                        </Link>
                                    </li>
                                    <li>
                                        <Link to={ '/counter' } activeClassName='active'>
                                            <span className='glyphicon glyphicon-education'></span> Counter
                                        </Link>
                                    </li>
                                    <li>
                                        <Link to={ '/fetchdata' } activeClassName='active'>
                                            <span className='glyphicon glyphicon-th-list'></span> Fetch data
                                        </Link>
                                    </li>
                                    <li>
                                        <Link to={ '/mycert' } activeClassName='active'>
                                            <span className=''></span> My Certaincy
                                        </Link>
                                    </li>
                                    <li>
                                        <Link to={ '/ourcert' } activeClassName='active'>
                                            <span className=''></span> Our Certaincy
                                        </Link>
                                    </li>
                                    <li>
                                        <Link to={ '/consultmap' } activeClassName='active'>
                                            <span className=''></span> Consult Map
                                        </Link>
                                    </li>
                                    <li>
                                        <Link to={ '/costumer' } activeClassName='active'>
                                            <span className=''></span> Costumer
                                        </Link>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
            </div>
    }
}
