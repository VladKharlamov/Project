$(document).ready(function () {
    $('.view-photo').on('click',
        function (e) {
            e.preventDefault();

            $.ajax({
                url: $(this).attr('href'),
                complete: function (response) {
                    var html = response.responseText, element = $(html);
                    var photoContent = element.find('#photo');
                    showPhoto(photoContent);
                }
            });
        });

    function showPhoto(photo) {
        $('body').addClass('view-photo');
        var $shadow = $('<div>', { class: 'shadow' }).appendTo('body');
        $shadow.html(photo);
        $shadow.append('<span class="glyphicon glyphicon-remove remove"></span>');
    }

    $(document).on('click',
        '.shadow .remove',
        function(e) {
            $('.shadow').remove();
            $('body').removeClass('view-photo');
        });
})