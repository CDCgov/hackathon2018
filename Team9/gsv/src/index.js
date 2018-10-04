$ = require('jquery');
require('@fortawesome/fontawesome-free');
require('uswds');

$(function(){
  $('.load-data').on('click', function(){
    $('.page').slideUp();
    $('#page-data').slideDown();
  });

  $('.load-about').on('click', function(){
    $('.page').slideUp();
    $('#page-about').slideDown();
  });

  $('#data-submit').on('click', function(e){
    $('#response-loading').slideDown();

    var url = $('#data-url').val();

    $.ajax({
      url: '/describe',
      data: {'url': url},
      method: 'POST',
      success: function(response){
        $('#response-loading').slideUp();
        $('#data-report').html(response.responseText).parent().slideDown();
      },
      error: function(xhr, status, text){
        $('#response-loading').slideUp();
        $('#data-report').parent().slideDown();
        // var re = $('#response-error');
        // re.find('.usa-alert-heading').text(status);
        // re.find('.usa-alert-text').text(text);
        // re.slideDown();
      }
    });

    $.ajax({
      url: '/manifest',
      data: {'url': url},
      method: 'POST',
      success: function(response){
        $('#response-loading').slideUp();
        $('#data-manifest').html(response.responseText).parent().slideDown();
      },
      error: function(xhr, status, text){
        $('#response-loading').slideUp();
        $('#data-manifest').parent().slideDown();
        // var re = $('#response-error');
        // re.find('.usa-alert-heading').text(status);
        // re.find('.usa-alert-text').text(text);
        // re.slideDown();
      }
    });
  });
});
