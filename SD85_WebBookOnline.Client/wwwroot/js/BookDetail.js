$(document).ready(function () {

    $('body').on('click', '#btn_add', function () {
        setData(null);
        $('#pp_Modal').modal('show')
    })

    $('body').on('click', '#btn_chitiet', function () {
        $('#pp_Modal').modal('show')
    })

    $('body').on('click', '#btn_xoa', function () {
        $('#pp_Modal').modal('show')
    })
    $('body').on('click', '#btn_luu', function () {
        if ($('#btn_bookdetailID').val() == null || $('#btn_bookdetailID').val() == undefined || $('#btn_bookdetailID').val() == '')
        {
            
            $.post('/add-bookDetail', { bk: getData() }, function (re) {
                if (re.status) {
                    alert(re.message);
                    $('#pp_Modal').modal('hide')
                }
                else {
                        alert(re.message)
                }
            })
        }
        
    })
   

})
function setData(data) {
    if (data != null && data != undefined && data != '') {
        $('#btn_bookdetailID').val(data.BookDetailID);
        $('#cb_bookname').val(data.bookID);
        $('#cb_category').val(data.categoryID);
        $('#cb_author').val(data.authorID);
        $('#cb_languge').val(data.LangugeID);
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
        BookID: $('#cb_bookname').val(),
        CategoryID: $('#cb_category').val(),
        AuthorID: $('#cb_author').val(),
        LangugeID: $('#cb_languge').val()
    }                  
}