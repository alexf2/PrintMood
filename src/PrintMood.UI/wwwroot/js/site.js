/*** Pace debug validation ***/
/*(function () {
    var count = 0;

    var f = function () {
        console.log($('.pace-running').length);
        console.log($('.pace').length);
        console.log($('.pace-done').length);
        console.log('---' + count);

        if (++count < 10) {
            console.log('next->');
            setTimeout(f, 400);
        }
    }
    setTimeout(f, 400);
})();*/


/***Header Effect**/
(function() {

    var $head = $('#ha-header'),
        $vp = $('.ha-waypoint'),
        animClassDown = $vp.data('animateDown'),
        animClassUp = $vp.data('animateUp'),
        scrollPos = $(document).scrollTop();
        
    Pace.once("done", function(){
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
        if (location.pathname.replace(/^\//, '') === this.pathname.replace(/^\//, '')
            || location.hostname === this.hostname) {

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
    $("#contact_form").validate();
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

/***** language selector *****/
(function ($) {
    var $root = $('div.lang-box-root'),
        $box = $root.find('div.lang-box'),
        $panel = $root.find('div.lang-list');

    $box.on('click.langHandler', function (e) {
        $panel.slideToggle('slow').promise().then(function () { $box.toggleClass('lang-box-opened'); });
        return false;
    });

    $box.on('selectstart.langHandler', function (e) {
        e.stopPropagation();
        e.preventDefault();
    });

    var close = function() {
        $panel.slideUp('slowly').promise().then(function () { $box.removeClass('lang-box-opened'); });
    }

    $(document).on('click.langHandler', function (e) {

        if ($panel.is(':visible') && $(e.target).closest('div.lang-box-root').length === 0)
            close();

    }).on('keyup.langHandler', function (e) {

        if ($panel.is(':visible') && e.which === 27)
            close();
    });

})(jQuery);

/***** contact form *****/
(function ($) {
    var $form = $("#contact_form"),
        action = $form.attr('action'),
        $wrap = $("#contact_wrap");
    
    
    var showAlert = function(err, $tree) {
        var $alert = $(err ?
            '<div class="alert alert-danger" style="display:none"><button type="button" class="close" aria-hidden="true">&times;</button></div>' :
            '<div class="alert alert-success" style="display:none"><button type="button" class="close" aria-hidden="true">&times;</button></div>');

        $tree.appendTo($alert);
        $form.before($alert);
        $alert.find('button.close').on('click', function() {
            $alert.slideUp(700, function () {
                $alert.remove();
            });
        });        
        $alert.slideDown('slow');
    }

    var showLoader = function(show)
    {
        $wrap.find('.ajax-loader').remove();
        if (show) {
            var $loader = $('<div class="ajax-loader">&nbsp;</div>');
            $loader.prependTo($wrap);
        }
    }

    var responseToList = function (mime, data) {
        var res = '', count = 0;
        if (/application\/json/ig.test(mime)) {

            var obj = typeof data === 'object' ? data : JSON.parse(data);
            //var res = "<ul style='list-style: disc'>";
            for (var k in obj)
                if (obj.hasOwnProperty(k)) {
                    var item = obj[k];
                    if (Array.isArray(item))
                        for (var i = 0; i < item.length; ++i)
                            res += '<li>' + item[i] + '</li>';
                    else {
                        if (typeof item === 'string')
                            res += '<li>' + item + '</li>';
                        else 
                            continue;
                    }
                    ++count;
                }
            if (res === '')
                res = '[empty]';
        } else {
            if (typeof data === 'string' && data.length === 0)
                res = '[empty]';
            else
                res = '<li>' + data + '</li>';
        }

        return count > 1 ? ('<ul style="list-style: disc">' + res + '</ul>') : ('<ul>' + res + '</ul>');
    }

    $form.submit(function () {
        $wrap.find('div.alert').remove();

        $form.validate();
        if (!$form.valid())
            return false;

        var timerId = 0;

        $.post(
            {
                url: action,
                data: $form.serialize(),
                beforeSend: function () { timerId = setTimeout(function () { showLoader(true); }, 200) }
                //headers: { "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val() },
            })
            .done(function(data, textStatus, jqXHR) {                                
                showAlert(false, $(responseToList(jqXHR.getResponseHeader("content-type"), data)));
                $form[0].reset();
            })
            .fail(function(jqXHR, textStatus, errorThrown) {                
                console.log(textStatus);
                console.log(errorThrown);
                console.log(jqXHR.responseText);

                showAlert(true, $(responseToList(jqXHR.getResponseHeader("content-type"), jqXHR.responseText)));
            })
            .always(function() {
                clearTimeout(timerId);
                showLoader(false);
                window.recaptchaContact.reset();
            });

        return false;
    });
})(jQuery);

(function () {    
    var siteKey = "6Ldu_QgUAAAAAPFY6yQOtbH1sLN0ABHcZy9rb_hw";        

    function recaptchaMgr(containerId, theme, size) {
        var optWidgetId;

        this.create = function() {
            optWidgetId = grecaptcha.render(containerId, {sitekey: siteKey, theme: theme, size: size });
        }

        this.reset = function() {
            grecaptcha.reset(optWidgetId);
        }

        this.getId = function () { return containerId; }
        this.getWidgetId = function () { return optWidgetId; }
    }

    window.recaptchaMgr = recaptchaMgr;
})();

function initRecaptcha() {
    
    window.recaptchaContact = new window.recaptchaMgr('ContactCaptcha', 'dark', 'normal'),
    window.recaptchaOrder = new window.recaptchaMgr('OrderCaptcha', 'light', 'normal');

    recaptchaContact.create();
}

