jQuery(document).ready(function($){
  
  // PORTFOLIO IMAGE HOVER:
  var speed = 300;
  var portfolio_link_opacity = 1;
  var icon_link_opacity = 1;
  
  
  var tour_slide_width = 700;
  var actual_tour_selected = 1;
 
    $('.tour_pagenavi_left').click(function(){
    var clicked = $(this).attr('rel');
    
         $('#tour_nav a').removeClass('tour_nav_active');
     $('#tour_nav a').eq(clicked-1).addClass('tour_nav_active');
     
    $('#tour_slider ul').children('li').eq(clicked-1).css('display','block');
    $('#tour_slider ul').children('li').eq(clicked-1).css('height','auto');
    $('#tour_slider ul').stop().animate({left: tour_slide_width*(clicked-1)*-1}, 300, function() {
      var size = $('#tour_slider ul').children('li').size();
      for(var i =0; i< size; i++)
      {
      
        
        if(i != (clicked-1))
        {
     //   alert (clicked + " " + i);  
         $('#tour_slider ul').children('li').eq(i).css('height','7px');
        }
      }
    });
    
    //alert()
    return false;
  } ); 
  
  
   $('.tour_pagenavi_right').click(function(){
       
    var clicked = $(this).attr('rel');
    
     $('#tour_nav a').removeClass('tour_nav_active');
     $('#tour_nav a').eq(clicked-1).addClass('tour_nav_active');
    $('#tour_slider ul').children('li').eq(clicked-1).css('display','block');
    $('#tour_slider ul').children('li').eq(clicked-1).css('height','auto');
    $('#tour_slider ul').stop().animate({left: tour_slide_width*(clicked-1)*-1}, 300, function() {
      var size = $('#tour_slider ul').children('li').size();
      for(var i =0; i< size; i++)
      {
      
        
        if(i != (clicked-1))
        {
     //   alert (clicked + " " + i);  
         $('#tour_slider ul').children('li').eq(i).css('height','7px');
        }
      }
    });
    
    //alert()
    return false;
  } );
  
  $('#tour_nav a').click(function(){
     $('#tour_nav a').removeClass('tour_nav_active');
     $(this).addClass('tour_nav_active');
    var clicked = $(this).attr('rel');
    $('#tour_slider ul').children('li').eq(clicked-1).css('display','block');
    $('#tour_slider ul').children('li').eq(clicked-1).css('height','auto');
    $('#tour_slider ul').stop().animate({left: tour_slide_width*(clicked-1)*-1}, 300, function() {
      var size = $('#tour_slider ul').children('li').size();
      for(var i =0; i< size; i++)
      {
      
        
        if(i != (clicked-1))
        {
     //   alert (clicked + " " + i);  
         $('#tour_slider ul').children('li').eq(i).css('height','7px');
        }
      }
    });
    
    //alert()
    return false;
  } );
  

  $('.widget_sidebar_content .menu li').hover(function(){
   $(this).children('.sub-menu').stop().slideDown(100);//.css('display', 'block');
  },function(){
   $(this).children('.sub-menu').stop().slideUp(0);
  }); 


  if($.browser.msie)
  { portfolio_link_opacity = 0.2;
    icon_link_opacity = 0.65;
  }
   $('.wp-caption a').click(function(){
    $.prettyPhoto.open($(this).attr('href'),'','' );
    return false;
   });
   $('.gallery-icon').find('a').click(function(){
    $.prettyPhoto.open($(this).attr('href'),'','');
    
   return false;
   });
  $('#fc_submit').click(function(){
     var send_email = true;
    var filter = /^([a-zA-Z0-9_.-])+@(([a-zA-Z0-9-])+.)+([a-zA-Z0-9]{2,4})+$/;
        

    if (!filter.test($('#fc_email').attr('value')) || $('#fc_email').attr('value') == '') {
        $('#fc_email').animate({left:-10},100).animate({left:10},100).animate({left:-10},100).animate({left:10},100).animate({left:0},100);
       
    /*    $('#cf_mail').animate({borderTop: '1px solid #ff0000'},{duration:5000, queue:false}); 
        $('#cf_mail').animate({borderLeft: '1px solid #ff0000'},{duration:5000, queue:false});  
        $('#cf_mail').animate({borderRight: '1px solid #ff0000'},{duration:5000, queue:false});
        $('#cf_mail').animate({borderBottom: '1px solid #ff0000'},{duration:5000, queue:false});    */
       send_email = false;  
    }
 
    if( $('#fc_name').attr('value') == '' ) { 
      $('#fc_name').animate({left:-10},100).animate({left:10},100).animate({left:-10},100).animate({left:10},100).animate({left:0},100);
     
      send_email = false;  
    }
   
    if( $('#fc_text').attr('value') == '' ) {
      $('#fc_text').animate({left:-10},100).animate({left:10},100).animate({left:-10},100).animate({left:10},100).animate({left:0},100);
     
      send_email = false;
    }
 
    if(send_email == false){ return false;}    
  } );
  
  
  $('.portfolio_link').hover(function(){
  $(this).find('img').stop().animate({opacity:0.5}, speed);
  /*
    $(this).find('.portfolio_hover').css('opacity', '0');
    $(this).find('.portfolio_hover').css('display', 'block');
    $(this).find('.portfolio_hover').stop().animate({opacity:portfolio_link_opacity},speed);
    
    $(this).find('.icon_zoom').css('opacity', '0');
    $(this).find('.icon_zoom').css('display', 'block');
    $(this).find('.icon_zoom').stop().animate({opacity:icon_link_opacity},speed);
    
    $(this).find('.icon_play').css('opacity', '0');
    $(this).find('.icon_play').css('display', 'block');
    $(this).find('.icon_play').stop().animate({opacity:icon_link_opacity},speed);        */
    
  },function(){   
  $(this).find('img').stop().animate({opacity:1}, speed);
  /*
    var div_holder = $(this).find('div');
    $(this).find('.portfolio_hover').stop().animate({opacity:0},speed, function(){ div_holder.css('display','none'); });
    $(this).find('.icon_zoom').stop().animate({opacity:0},speed, function(){ div_holder.css('display','none'); });
    $(this).find('.icon_play').stop().animate({opacity:0},speed, function(){ div_holder.css('display','none'); });                */
  });
  
    $('.gallery_link').hover(function(){
    $(this).find('img').stop().animate({opacity:0.5}, speed);/*
    $(this).find('.gallery_hover').css('opacity', '0');
    $(this).find('.gallery_hover').css('display', 'block');
    $(this).find('.gallery_hover').stop().animate({opacity:portfolio_link_opacity},speed);
    
    $(this).find('.icon_zoom').css('opacity', '0');
    $(this).find('.icon_zoom').css('display', 'block');
    $(this).find('.icon_zoom').stop().animate({opacity:icon_link_opacity},speed);
    
    $(this).find('.icon_play').css('opacity', '0');
    $(this).find('.icon_play').css('display', 'block');
    $(this).find('.icon_play').stop().animate({opacity:icon_link_opacity},speed);     */
    
  },function(){ 
    $(this).find('img').stop().animate({opacity:1}, speed);                                                                           /*
    var div_holder = $(this).find('div');
    $(this).find('.gallery_hover').stop().animate({opacity:0},speed, function(){ div_holder.css('display','none'); });
    $(this).find('.icon_zoom').stop().animate({opacity:0},speed, function(){ div_holder.css('display','none'); });
    $(this).find('.icon_play').stop().animate({opacity:0},speed, function(){ div_holder.css('display','none'); });  */
  });
	jQuery(".toggle_body").hide(); 

	jQuery("h4.toggle").toggle(function(){
		jQuery(this).addClass("toggle_active");
		}, function () {
		jQuery(this).removeClass("toggle_active");
	});
	
	jQuery("h4.toggle").click(function(){
		jQuery(this).next(".toggle_body").slideToggle();

	});
 
// NAVIGATION HOVER!

 $('.sc_tab').click(function() {
    $('.sc_tab').removeClass('sc_tab_active');
    $(this).addClass('sc_tab_active');
    var which = $(this).attr('title');
    $(this).parent().parent().find('.sc_tab_single_box').css('display','none'); 
    $(this).parent().parent().find('.sc_tab_single_box').eq(which).css('display','block');
  });

  $('.menu-item').click(function() {
    //window.location('http://www.freshface.cz');
    window.location = $(this).find('a:first').attr('href');
    //alert(';dsds');
    //window.location($(this).find('a:first').attr('href'));
  });
    var dropdown_level = 0;
    
    $('.sub-menu').parent().find('a:first').addClass('nav_sub_arrow');
    $('#nav_wrapper .menu').children('li').children('a').addClass('top_level');
    $('#nav_wrapper .menu').children('li').children('a').removeClass('nav_sub_arrow');
    
    $('#nav_wrapper .menu-item').hover(function(){
      if(dropdown_level == 0){
            $('#nav_wrapper .menu').find('a').removeClass('nav_sub_arrow_active');  
            //$('#nav_wrapper .menu').find('a').addClass('nav_sub_arrow_passive');  
          $(this).addClass('main_hover_left');
          $(this).children('a').addClass('main_hover_right');
            $('.sub-menu').parent().find('a:first').addClass('nav_sub_arrow');
        $('#nav_wrapper .menu').children('li').children('a').addClass('top_level');
        $('#nav_wrapper .menu').children('li').children('a').removeClass('nav_sub_arrow');
      //alert('sasa');
      }                                                                                             
       
                  Cufon.replace('#nav_wrapper ul.sub-menu li a', {
fontFamily: 'Myriad Pro Semibold',
textShadow: 'none',
hover: 'true'
});
      $(this).find('.sub-menu:first').stop(true,true).slideDown(200).show();        
      $(this).find('a:first').addClass('nav_sub_arrow_active');
      $('#nav_wrapper .menu').children('li').children('a').removeClass('nav_sub_arrow_active');       
      dropdown_level++;
    },function(){            
      $(this).find('.sub-menu:first').stop(true,true).slideUp(0);
      $(this).find('a:first').removeClass('nav_sub_arrow_active');
      dropdown_level--;
       if(dropdown_level == 0){
        $(this).removeClass('main_hover_left');
          $(this).children('a').removeClass('main_hover_right');
       }
    } );
  /*  $('.sub-menu').parent().find('a:first').addClass('nav_sub_arrow');    
    $('.menu').children('li').children('a').removeClass('nav_sub_arrow');
     $('.menu').children('li').children('a').addClass('top_level');
    $('.menu').find('a').addClass('sub_menu_active');
    $('.top_level').removeClass('sub_menu_active');
    
  $('#nav_wrapper .menu-item').hover(function(){

//   alert('ds');
    if($(this).children('a:first').hasClass('top_level') == true)
    {
    //  $(this).find('a').removeClass('nav_sub_arrow_active');
      $(this).addClass('main_hover_left');
      $(this).find('a:first').addClass('main_hover_right');
    }
    else
    {
     
    }
     $(this).parent().parent().addClass('nav_sub_arrow_active');
    // $(this).find('.sub-menu').parent().children('a:first').addClass('nav_sub_arrow_active');
    dropdown_level++;
    // DROPDOWN //
    $(this).find('.sub-menu:first').stop(true,true).slideDown(200).show();
  },function(){
    dropdown_level--;
    $(this).children('a:first').removeClass('nav_sub_arrow_active');
    $(this).removeClass('main_hover_left');
    $(this).find('a:first').removeClass('main_hover_right');
    if(dropdown_level== 0)
    {
        $('#nav_wrapper .menu-item').find('a').removeClass('nav_sub_arrow_active');
    }
    //alert(dropdown_level);
    // DROPDOWN //
    
    $(this).find('.sub-menu:first').stop(true,true).slideUp(0);
  } );
                                              */
  $('#submit_contactform').click(function(){
    var send_email = true;
    var filter = /^([a-zA-Z0-9_.-])+@(([a-zA-Z0-9-])+.)+([a-zA-Z0-9]{2,4})+$/;
        

    if (!filter.test($('#cf_mail').attr('value')) || $('#cf_mail').attr('value') == '') {
        $('#cf_mail').animate({left:-10},100).animate({left:10},100).animate({left:-10},100).animate({left:10},100).animate({left:0},100);
        $('#cf_mail').addClass('input_alert');
    /*    $('#cf_mail').animate({borderTop: '1px solid #ff0000'},{duration:5000, queue:false}); 
        $('#cf_mail').animate({borderLeft: '1px solid #ff0000'},{duration:5000, queue:false});  
        $('#cf_mail').animate({borderRight: '1px solid #ff0000'},{duration:5000, queue:false});
        $('#cf_mail').animate({borderBottom: '1px solid #ff0000'},{duration:5000, queue:false});    */
       send_email = false;  
    }
       else
    {
      $('#cf_mail').removeClass('input_alert');
    }
            
    if( $('#cf_name').attr('value') == '' ) { 
      $('#cf_name').animate({left:-10},100).animate({left:10},100).animate({left:-10},100).animate({left:10},100).animate({left:0},100);
      $('#cf_name').addClass('input_alert');
      send_email = false;  
    }
    else
    {
      $('#cf_name').removeClass('input_alert');
    }
    
    if( $('#cf_text').attr('value') == '' ) {
      $('#cf_text').animate({left:-10},100).animate({left:10},100).animate({left:-10},100).animate({left:10},100).animate({left:0},100);
      $('#cf_text').addClass('input_alert');
      send_email = false;
    }
       else
    {
      $('#cf_text').removeClass('input_alert');
    }
    if(send_email == false){$('.contact_form .btn_a').removeClass('btn_a').addClass('btn_a'); return false;}      

  } );
  $('#submit').click(function(){
  
       var send_email = true;
    var filter = /^([a-zA-Z0-9_.-])+@(([a-zA-Z0-9-])+.)+([a-zA-Z0-9]{2,4})+$/;
    if($(this).parent().parent().find('.admin_checker').attr('title') != 'admin')
    {    

      if (!filter.test($('#email').attr('value')) || $('#email').attr('value') == '') {
          $('#email').animate({left:-10},100).animate({left:10},100).animate({left:-10},100).animate({left:10},100).animate({left:0},100);
         send_email = false;  
      }
              
      if( $('#author').attr('value') == '' ) { 
        $('#author').animate({left:-10},100).animate({left:10},100).animate({left:-10},100).animate({left:10},100).animate({left:0},100);
        send_email = false;  
      }
    }
    if( $('#comment').attr('value') == '' ) {
      $('#comment').animate({left:-10},100).animate({left:10},100).animate({left:-10},100).animate({left:10},100).animate({left:0},100);
      send_email = false;
    }
    if(send_email == false){$('#submit').removeClass('btn_a').addClass('btn_a'); return false;}      
  });
  
  
});