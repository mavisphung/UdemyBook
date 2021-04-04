function resize() {

    //apply for iphone ipad
    if ($(window).width() > 540 && $(window).width() <= 768) {
        $("#column-3rd").addClass("offset-3");
        $("#column-3rd").addClass("my-3");
        $("#column-4th").addClass("offset-3");
        $("#column-4th").addClass("my-3");
    } else {
        $("#column-3rd").removeClass("offset-3");
        $("#column-3rd").removeClass("my-3");
        $("#column-4th").removeClass("offset-3");
        $("#column-4th").removeClass("my-3");
    }
}

$(document).ready(function () {
    $(window).resize(resize);
    resize();
    $("#Development").addClass("active").addClass("show");
    $(".owl-carousel").owlCarousel({
        loop: true,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: true,
                loop: true,
            },
            600: {
                items: 3,
                nav: false,
                loop: true,
            },
            1000: {
                items: 4,
                nav: true,
                loop: true,
            },
        },
        dots: false,
        nav: false,
        navText: ["", ""],
        margin: 10,
    });
});

//function setActive() {
//    var develop = document.getElementById("#Development");
//    if (develop == null) {
//        console.log("Tag này bị lỗi rồi");
//    } else {
//        develop.addClass("active");
//    }
//}