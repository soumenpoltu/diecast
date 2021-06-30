(function ($) {
 "use strict";

  $(".menu-button").click(function(){
    $(".sidepanel").addClass("showmenu");
  });
  $(".closeit").click(function(){
    $(".sidepanel").removeClass("showmenu");
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
   
   
   /*--------------------------------------*/
   /*Images alt Title width height
   /*------------------------------------*/
           $(document).ready(function () {
                $('img').each(function () {
                    var $img = $(this);
                    var filename = $img.attr('src')
                    if (typeof attr == typeof undefined || attr == false) {
                        $img.attr('alt', 'Diecast Decals');
                        $img.attr('title', 'Diecast Decals');
                        $img.attr('width', this.width);
                        $img.attr('height', this.height);
                    }
                });
            }); 

})(jQuery);    