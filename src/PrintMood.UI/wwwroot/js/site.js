/***Header Effect**/
(function() {

    var $head = $('#ha-header'),
        $vp = $('.ha-waypoint'),
        animClassDown = $vp.data('animateDown'),
        animClassUp = $vp.data('animateUp'),
        scrollPos = $(document).scrollTop();
        
    Pace.on("done", function(){
        $('#ha-header').attr('class', 'ha-header ha-header-large');
    });    

    $(document).on("scroll",function() {
        var pos = $(document).scrollTop();        

        if(pos > scrollPos + 10) {
            scrollPos = pos;
            if (!$head.hasClass(animClassDown))
                $head.removeClass(animClassUp).addClass("ha-header").addClass(animClassDown);
        } 
        else if(pos < scrollPos - 10){
            scrollPos = pos;            
            if (!$head.hasClass(animClassUp))
                $head.removeClass(animClassDown).addClass("ha-header").addClass(animClassUp);
        }
    }); 
})();


(function() {
    $(document).ready(function() {
        fixFlexsliderHeight();
    });

    $(window).load(function() {
        fixFlexsliderHeight();
    });

    $(window).resize(function() {
        fixFlexsliderHeight();
    });

    function fixFlexsliderHeight() {        
        // Set fixed height based on the tallest slide
        $('.flexslider').each(function(){
            var sliderHeight = 99999;
            $(this).find('.slides > li').each(function(){
                sliderHeight = Math.min(sliderHeight, $(this).height());
            });

            $(this).find('ul.slides').css({'height' : sliderHeight + "px"});
            $(this).parent().css({'height' : sliderHeight + "px"});            
        });        
    }
})();

(function() {

    /*******Nice Scroll******/
    $("html").niceScroll({
        cursoropacitymin: 0,
        cursoropacitymax: .8,
        cursorcolor: "#ca4549",
        cursorwidth: "9px",
        cursorborder: "0",
        cursorborderradius: "0",
        scrollspeed: 60,
        mousescrollstep: 8 * 3,
        autohidemode: false,
        horizrailenabled: false,
        background: "#F1F1F1",
        zindex: 9999        
    }).resize(); // The document page (body)    

    /* flexs lider */
        $('.flexslider').flexslider({
            animation: "fade",
            controlNav: false,
            smoothHeight: false,
            startAt: 1,
            start: function(slider) {
                $('body').removeClass('loading');
            },
            prevText: $('[name = "texts.prev"]').val(),
            nextText: $('[name = "texts.next"]').val()
        });

    $('.flexslider .flex-prev, .flexslider .flex-next').each(function(idx, el) {
        var $el = $(el);
        $el.attr('title', $el.text());
        $el.text('');
    });


    /***Hover Effect with mask**/
    $('span.mask').hover(
        function() {
            $(this).siblings('a img').addClass('hovering');
            $(this).parent().siblings(".portfolio-title").children("h4").stop().animate({
                top: -20
            }, 350);
        },
        function() {
            $(this).siblings('a img').removeClass('hovering');
            $(this).parent().siblings(".portfolio-title").children("h4").stop().animate({
                top: 0
            }, 350);
        }
    );


    /****Smooth Scrolling***/
    $('a[href*=\\#]:not([href=\\#])').click(function() {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '')
            || location.hostname == this.hostname) {

            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                $('html,body').animate({
                    scrollTop: target.offset().top
                }, 1000);
                return false;
            }
        }
    });


    /*contact    page    validator*/
    $("#passion_form").validate();	    
})();

function initGMap() {
    /*****google map*****/
    
    var map;
    map = new GMaps({
        el: '#map',
        lat: 48.126408, lng: 17.222549,        
        zoomControl: true,
        zoomControlOpt: {
            style: 'SMALL',
            position: 'TOP_LEFT'
        },
        panControl: true,
        streetViewControl: true,
        mapTypeControl: true,
        overviewMapControl: true,
        fullscreenControl: true,
        zoom: 15
    });    

    map.addMarker({
        lat: 48.126408,
        lng: 17.222549,
        title: 'Print Mood',
        infoWindow: { content: '<div class="google-marker"><strong>Print Mood</strong><p>82106 Slovakia, Bratislava, Pri trati, 25A</p></div>' }
    });
};
