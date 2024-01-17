$(document).ready(function () {

    $('body').on('click', '#btn_add_detail', function () {
        let id = $(this).attr('data-id');
      
        setData('');
        $('#cb_bookname').val(id);
         $('#pp_Modal_detail').modal('show');

          
        })
    })

$('body').on('click', '#btn_chitiet_detail', function () {
        let id = $(this).attr('data-id');
        $.get('/detail-bookdetail/' + id, function (re) {
            if (re.status) {
                setData(re.data)
                $('#pp_Modal_detail').modal('show');

            }
            else {
                alert(re.message)
            }
        })

    })
    $('body').on('click', '#btn_luu', function () {
        if ($('#btn_bookdetailID').val() == null || $('#btn_bookdetailID').val() == undefined || $('#btn_bookdetailID').val() == '') {

            $.post('/add-bookDetail', { bk: getData() }, function (re) {
                if (re.status) {
                    alert(re.message);
                    $('#pp_Modal_detail').modal('hide');
                    window.location.reload()
                }
                else {
                    alert(re.message)
                }
            })
        }
        else {
            let id = $(this).attr('data-id')
            $.post('/UpdateBookDetail/' + id, { vc: getData() }, function (re) {

                if (re.status) {
                    alert(re.message);
                    $('#pp_Modal_detail').modal('hide');
                    window.location.reload()
                }
                else {
                    alert(re.message)
                }
            })
        }
    })

function setData(data) {
    if (data != null && data != undefined && data != '') {
        $('#btn_bookdetailID').val(data.bookDetailID);
        $('#cb_bookname').val(data.bookID);
        $('#cb_category').val(data.categoriesID);
        $('#cb_author').val(data.authorID);
        $('#cb_languge').val(data.lagugeID);
    }
    else {
        $('#btn_bookdetailID').val('');
        $('#cb_bookname').val('');
        $('#cb_category').val('');
        $('#cb_author').val('');
        $('#cb_languge').val('');
    }
}
function getData() {
    return {
        /* BookDetailID: $('#btn_bookdetailID').val(),*/
        BookID: $('#cb_bookname').val(),
        CategoriesID: $('#cb_category').val(),
        AuthorID: $('#cb_author').val(),
        LagugeID: $('#cb_languge').val()
    }
}