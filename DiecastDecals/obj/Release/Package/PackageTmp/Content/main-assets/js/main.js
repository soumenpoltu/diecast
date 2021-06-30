(function ($) {
 "use strict";

   



  $(".menu-button").click(function(){
    $(".sidepanel").addClass("showmenu");
  });
  $(".closeit").click(function(){
    $(".sidepanel").removeClass("showmenu");
  });


    //sidepanel
    $(".sidepanel ul li > .fa-angle-right").click(function () {
        $(this).toggleClass("rotatedown");
        $(this).next("ul").toggle("slow");
    });


    //$('.categoris.categori-border ul li span.filtercheckbox ').click(function () {
    //    if ($(this).find("input[type='checkbox']").prop("checked") == true) { 
    //        $(this).next("ul").show("slow");
    //        $(this).addClass("clickedcat");
            
    //    }
    //    else if ($(this).find("input[type='checkbox']").prop("checked") == false) { 
    //        $(this).next("ul").hide("slow");
    //        $(this).removeClass("clickedcat");
    //    }
    //});

  //show password single password field in single page
  $(".showpass-single input[type='checkbox']").on("click", function(){
    if($(this).is(":checked")){
            $("input.password-single").attr("type","text");
        }
        else if($(this).is(":not(:checked)")){
           $("input.password-single").attr("type","password");
        }
  }); 
  
  //show password multiple password field in single page
  $(".showpass-a input[type='checkbox']").on("click", function(){
    if($(this).is(":checked")){
            $("input.password-a").attr("type","text");
        }
        else if($(this).is(":not(:checked)")){
           $("input.password-a").attr("type","password");
        }
  });  
  
  //show password multiple password field in single page
  $(".showpass-b input[type='checkbox']").on("click", function(){
    if($(this).is(":checked")){
            $("input.password-b").attr("type","text");
        }
        else if($(this).is(":not(:checked)")){
           $("input.password-b").attr("type","password");
        }
  });  
   
  
/*--------------------------------
 slide product
---------------------------------- */   
     $(".slide-product").owlCarousel({
      navigation : true, 
      pagination : false,
      slideSpeed : 600,
      paginationSpeed : 400,
      items : 2,
      itemsDesktop : [1199,1],
      itemsDesktopSmall : [979,2], 
      itemsTablet: [767,2], 
      itemsMobile : [480,1],
      navigationText : ['<i class="icon-left-open"><i class="fa fa-angle-left"></i></i>','<i class="icon-right-open"><i class="fa fa-angle-right"></i></i>'] 
  });
/*--------------------------------
 featured item
---------------------------------- */
     $(".featured-item").owlCarousel({
      navigation : true,
      pagination : false,
      slideSpeed : 600,
      paginationSpeed : 400,
    
      itemsDesktop : [1201,4],
      itemsDesktopSmall : [992,3], 
      itemsTablet: [768,2], 
      itemsMobile : [480,1],
      navigationText : ['<i class="icon-left-open"><i class="fa fa-angle-left"></i></i>','<i class="icon-right-open"><i class="fa fa-angle-right"></i></i>'], 
      responsive:{
        0:{
            items:1
        },
        600:{
            items:3
        },
        1000:{
            items:5
        }
    }
  });    
/*--------------------------------
 camera-camcord
---------------------------------- */
     $(".most-featured-item").owlCarousel({
      navigation : true,
      pagination : false,
      slideSpeed : 600,
      paginationSpeed : 400,
     
      itemsDesktop : [1201,4],
      itemsDesktopSmall : [992,3], 
      itemsTablet: [768,2], 
      itemsMobile : [480,1],
      navigationText : ['<i class="icon-left-open"><i class="fa fa-angle-left"></i></i>','<i class="icon-right-open"><i class="fa fa-angle-right"></i></i>'],
      responsive:{
        0:{
            items:1
        },
        600:{
            items:3
        },
        1000:{
            items:5
        }
    }
    }); 
/*--------------------------------
 popular tab product
---------------------------------- */
$(".popular-tab-product").owlCarousel({
      navigation : true,
      pagination : false,
      slideSpeed : 600,
      paginationSpeed : 400,
      items : 5,
      itemsDesktop : [1201,4],
      itemsDesktopSmall : [992,3], 
      itemsTablet: [768,2], 
      itemsMobile : [480,1],
      navigationText : ['<i class="icon-left-open"><i class="fa fa-angle-left"></i></i>','<i class="icon-right-open"><i class="fa fa-angle-right"></i></i>'],
	   responsive:{
        0:{
            items:1
        },
        600:{
            items:3
        },
        1000:{
            items:5
        }
    }
  });
/*--------------------------------
 popular tab product
---------------------------------- */
    var owl = $(".popular-tab-product-4");
      owl.owlCarousel({
      navigation:true,
      slideSpeed : 600,
      pagination : false,
      addClassActive : true,
      lazyLoad : true,
      items :3,
      itemsDesktop : [1024,3],
      itemsDesktopSmall : [980,3], 
      itemsTablet: [767,2], 
      itemsMobile : [480,1],
      navigationText : ['<i class="icon-left-open"><i class="fa fa-angle-left"></i></i>','<i class="icon-right-open"><i class="fa fa-angle-right"></i></i>']
  });      
/*--------------------------------
 brand-logo
---------------------------------- */
     $(".brand-logo").owlCarousel({
      navigation : true,
      pagination : false,
      slideSpeed : 600,
      paginationSpeed : 400,
      items : 6,
      itemsDesktop : [1199,6],
      itemsDesktopSmall : [979,4], 
      itemsTablet: [767,3], 
      itemsMobile : [480,1],
      navigationText : ['<i class="icon-left-open"><i class="fa fa-angle-left"></i></i>','<i class="icon-right-open"><i class="fa fa-angle-right"></i></i>'] 
  });
/*--------------------------------
 blog-box
---------------------------------- */
     $(".blog-box").owlCarousel({
      navigation : true,
      pagination : false,
      slideSpeed : 600,
      paginationSpeed : 400,
      items : 5,
      itemsDesktop : [1201,4],
      itemsDesktopSmall : [992,3], 
      itemsTablet: [768,2], 
      itemsMobile : [480,1],
      navigationText : ['<i class="icon-left-open"><i class="fa fa-angle-left"></i></i>','<i class="icon-right-open"><i class="fa fa-angle-right"></i></i>'],
	   responsive:{
        0:{
            items:1
           },
           768: {
               items: 2
           },
           991: {
               items: 3
           },
           1000: {
               items:3
           },
           1200: {
               items: 4
           },
           1400: {
               items: 4
           },
           1600: {
               items: 5
           }
    }
  });
/*--------------------------------
 blog-box
---------------------------------- */
     $(".tab-info-product").owlCarousel({
      navigation : false,
      pagination : false,
      slideSpeed : 600,
      paginationSpeed : 400,
      items : 4,
      itemsDesktop : [1199,4],
      itemsDesktopSmall : [979,3], 
      itemsTablet: [767,2], 
      itemsMobile : [480,1],
  });
/*--------------------------------
 about-su-active
---------------------------------- */
    $(".about-us-slide").owlCarousel({
      autoPlay:10000,
      paginationSpeed : 1000,
      pagination : false,
      items : 1,
      itemsDesktop : [1199,1],
      itemsDesktopSmall : [979,1],
      itemsTablet: [767,1]

    });
/*--------------------------------
 viewproduct
---------------------------------- */
    $("#viewproduct").owlCarousel({
      autoPlay:10000,
      paginationSpeed : 1000,
      pagination : false,
      items : 5,
      itemsDesktop : [1199,4],
      itemsDesktopSmall : [979,3],
      itemsTablet: [767,4],
      itemsMobile : [480,2]
    });   
/*--------------------------------
 mobile menu tab
---------------------------------- */
  jQuery('nav#dropdown').meanmenu();          
/*--------------------------------
 nivoSlider
---------------------------------- */     
  $('#slider').nivoSlider({
        effect: 'random',
        slices: 15,
        boxCols: 8,
        boxRows: 4,
        animSpeed: '600',
        pauseTime: '6000000',
        startSlide: 0,
        directionNav: 1,
        controlNav: 1,
        controlNavThumbs: false,
        pauseOnHover: false,
        manualAdvance: false,
        prevText: '<i class="fa fa-angle-left nivo-prev-icon"></i>',
        nextText: '<i class="fa fa-angle-right nivo-next-icon"></i>'
    });
/*--------------------------------
 tabs
---------------------------------- */  
  $('#tabs2').tab();
/*---------------------
 scrollUp
--------------------- */  
  $.scrollUp({
      scrollText: '<i class="fa fa-angle-double-up"></i>',
      easingType: 'linear',
      scrollSpeed: 900,
      animation: 'fade'
  });
/*--------------------------------
 owlCarousel5
---------------------------------- */
  $('.fancybox').fancybox();  

  /*----------------------------------------*/
	/* FAQ Accordion
/*----------------------------------------*/
	$('.card-header a').on('click', function() {
		$('.card').removeClass('actives');
		$(this).parents('.card').addClass('actives');
	});
	
	function wcqib_refresh_quantity_increments() {
    jQuery("div.quantity:not(.buttons_added), td.quantity:not(.buttons_added)").each(function(a, b) {
        var c = jQuery(b);
        c.addClass("buttons_added"), c.children().first().before('<input type="button" value="-" class="minus" />'), c.children().last().after('<input type="button" value="+" class="plus" />')
    })
}
String.prototype.getDecimals || (String.prototype.getDecimals = function() {
    var a = this,
        b = ("" + a).match(/(?:\.(\d+))?(?:[eE]([+-]?\d+))?$/);
    return b ? Math.max(0, (b[1] ? b[1].length : 0) - (b[2] ? +b[2] : 0)) : 0
}), jQuery(document).ready(function() {
    wcqib_refresh_quantity_increments()
}), jQuery(document).on("updated_wc_div", function() {
    wcqib_refresh_quantity_increments()
}), jQuery(document).on("click", ".plus, .minus", function() {
    var a = jQuery(this).closest(".quantity").find(".qty"),
        b = parseFloat(a.val()),
        c = parseFloat(a.attr("max")),
        d = parseFloat(a.attr("min")),
        e = a.attr("step");
    b && "" !== b && "NaN" !== b || (b = 0), "" !== c && "NaN" !== c || (c = ""), "" !== d && "NaN" !== d || (d = 0), "any" !== e && "" !== e && void 0 !== e && "NaN" !== parseFloat(e) || (e = 1), jQuery(this).is(".plus") ? c && b >= c ? a.val(c) : a.val((b + parseFloat(e)).toFixed(e.getDecimals())) : d && b <= d ? a.val(d) : b > 0 && a.val((b - parseFloat(e)).toFixed(e.getDecimals())), a.trigger("change")
});
   
})(jQuery);    

(function ($) {
 "use strict";

  $(".copylink").click(function(){
    $(".linkprofile").addClass("showmenu");
  });

  $('.copylink').click(function() {
    $(".linkprofile").focus();
    $(".linkprofile").select();
    document.execCommand('copy');
    $(".copied").text("Copied to clipboard").show().fadeOut(1200);
  });

})(jQuery);    

$(function(){
    Test = {
        UpdatePreview: function(obj){
          // if IE < 10 doesn't support FileReader
          if(!window.FileReader){
             // don't know how to proceed to assign src to image tag
          } else {
             var reader = new FileReader();
             var target = null;
             
             reader.onload = function(e) {
              target =  e.target || e.srcElement;
               $(".samplethumb").prop("src", target.result);
             };
              reader.readAsDataURL(obj.files[0]);
          }
        }
    };
});


$(function(){
    Testa = {
        UpdatePreview: function(obj){
          // if IE < 10 doesn't support FileReader
          if(!window.FileReader){
             // don't know how to proceed to assign src to image tag
          } else {
             var reader = new FileReader();
             var target = null;
             
             reader.onload = function(e) {
              target =  e.target || e.srcElement;
               $(".samplethumb-new").prop("src", target.result);
             };
              reader.readAsDataURL(obj.files[0]);
          }
        }
    };
});
