$(document).ready(function () {
  $('.button-collapse').sideNav({
      menuWidth: 250, // Default is 300
      closeOnClick: true, // Closes side-nav on <a> clicks, useful for Angular/Meteor
      draggable: true, // Choose whether you can drag to open on touch screens
  })
  $('.collapsible').collapsible()
  $('select').material_select()
  Materialize.updateTextFields()
})