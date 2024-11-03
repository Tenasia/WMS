$(document).ready(function() {
    // Smooth scrolling for navigation links
    $('a.nav-link').on('click', function(event) {
        if (this.hash !== "") {
            event.preventDefault();
            var hash = this.hash;
            $('html, body').animate({
                scrollTop: $(hash).offset().top - 56
            }, 800);
        }
    });

    // Form submission (you'll need to implement the actual form submission logic)
    $('#contact-form').on('submit', function(event) {
        event.preventDefault();
        // Implement form validation here
        var name = $('#name').val();
        var email = $('#email').val();
        var message = $('#message').val();

        if (name && email && message) {
            alert('Thank you for your message. We will get back to you soon!');
            this.reset();
        } else {
            alert('Please fill in all required fields.');
        }
    });

    // Newsletter subscription (you'll need to implement the actual subscription logic)
    $('#newsletter-form').on('submit', function(event) {
        event.preventDefault();
        var email = $(this).find('input[type="email"]').val();

        if (email) {
            alert('Thank you for subscribing to our newsletter!');
            this.reset();
        } else {
            alert('Please enter a valid email address.');
        }
    });

    // Add animation to cards on scroll
    $(window).scroll(function() {
        $('.card').each(function() {
            var bottom_of_object = $(this).offset().top + $(this).outerHeight();
            var bottom_of_window = $(window).scrollTop() + $(window).height();

            if (bottom_of_window > bottom_of_object) {
                $(this).animate({'opacity':'1'}, 500);
            }
        });
    });
});

