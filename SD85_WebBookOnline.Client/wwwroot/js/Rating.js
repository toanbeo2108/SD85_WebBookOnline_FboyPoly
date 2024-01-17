$(document).ready(function () {

    $('body').on('click', '#btn_xoa', function () {
        let id = $(this).attr('data-id')
        $.get('/delete_rating/' + id, function (re) {
            alert(re.message);
            window.location.reload();
        })

    })
    

});

