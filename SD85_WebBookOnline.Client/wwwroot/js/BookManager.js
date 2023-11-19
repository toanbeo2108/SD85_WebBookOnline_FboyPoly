$(document).ready(function () {
    $('body').on('click', '#btn_add', function () {
        setData(null)
        $('#pp_Modal').modal('show');
    })
    $('body').on('click', '#btn_save', function () {
        var send = getData();
         if ($('#btn_IdBoook').val() == null || $('#btn_IdBoook').val() == undefined || $('#btn_IdBoook').val() == '') {
        $.post('/Add-Book', { bk: send }, function (re) {
            if (re.status) {
                alert(re.message);
                $('#pp_Modal').modal('hide');
                window.location.reload();
            }
            else {
                alert(re.message);
            }
        })
        }
         else {
             let id = $('#btn_IdBoook').val();
             $.post('/update-Book/' + id, { vc: send }, function (re) {

                 if (re.status) {

                     alert(re.message);
                     $('#pp_Modal').modal('hide');

                     window.location.reload();
                 }
                 else {
                     alert(re.message);

                 }
             })
         }
    })
    $('body').on('click', '#btn_chitiet', function () {
        let id = $(this).attr('data-id')
        $.get('/detail-book/' + id, function (re) {
            $('#pp_Modal').modal('show');
            setData(re.data)
            
        })
    })
    $('body').on('click', '#btn_xoa', function () {
        let id = $(this).attr('data-id')
        $.post('/Dell-Book/' + id, function (re) {
            if (re.status) {
                alert(re.message)
                window.location.reload();
            }
            
        })
    })
   
})

function setData(data) {
    if (data == null || data == undefined || data == '') {


        $('#btn_IdBoook').val('');
        $('#cb_manu').val('');
        $('#cb_coupon').val('');
        $('#cb_form').val('');
        $('#btn_bookName').val('');
        $('#btn_ttqtt').val('');
        $('#btn_qttsold').val('');
        $('#btn_qttexit').val('');
        $('#btn_entryprice').val('');
        $('#btn_price').val('');
        $('#btn_in4').val('');
        $('#btn_Description').val('');
        $('#btn_ISBN').val('') ;
        $('#btn_Y_release').val('') ;
        $('#btn_Weight').val('') ;
        $('#btn_Volume').val('') ;
        $('#btn_TransactionStatus').val('') ;
        $('#btn_Status').val('') ;
        $('#btn_File').val('') ;
      
    }
    else {
       $('#btn_IdBoook').val(data.bookID);
        $('#cb_manu').val(data.manufacturerID);
        $('#cb_form').val(data.formID);
        $('#cb_coupon').val(data.couponID);
        $('#btn_bookName').val(data.bookName);
        $('#btn_ttqtt').val(data.totalQuantity);
        $('#btn_qttsold').val(data.quantitySold);
        $('#btn_qttexit').val(data.quantityExists);
        $('#btn_entryprice').val(data.entryPrice);
        $('#btn_price').val(data.price);
        $('#btn_in4').val(data.information);
        $('#btn_Description').val(data.description);
        $('#btn_ISBN').val(data.isbn);
        $('#btn_Y_release').val(data.yearOfRelease);
     
        $('#btn_TransactionStatus').val(data.transactionStatus);
        $('#btn_Status').val(data.status);
        $('#btn_Weight').val(data.weight);
        $('#btn_Volume').val(data.volume);
        $('#btn_File').val(data.mainPhoto);
    }
}

function getData() {
    return {
        BookID            : $('#btn_IdBoook').val(),
        ManufacturerID    : $('#cb_manu').val(),
        FormID            : $('#cb_form').val(),
        CouponID          : $('#cb_coupon').val(),
        BookName          : $('#btn_bookName').val(),
        TotalQuantity     : $('#btn_ttqtt').val(),
        MainPhoto         : $('#btn_File').val(),
        QuantitySold      : $('#btn_qttsold').val(),
        QuantityExists    : $('#btn_qttexit').val(),
        EntryPrice        : $('#btn_entryprice').val(),
        Price             : $('#btn_price').val(),
        Information       : $('#btn_in4').val(),
        Description       : $('#btn_Description').val(),
        ISBN              : $('#btn_ISBN').val(),
        YearOfRelease     : $('#btn_Y_release').val(),
        /*CreateDate        : $('#btn_Weight').val(),*/
        /*DeleteDate        : $('#btn_IdBoook').val(),*/
        TransactionStatus : $('#btn_TransactionStatus').val(),
        Status            : $('#btn_Status').val(),
        Weight            : $('#btn_IdBoook').val(),
        Volume            : $('#btn_Volume').val()
    }
   
}