﻿$(document).ready(function () {
    $('body').on('click', '#btn_add', function () {
        setdata(null);
        $('#pp_modal').modal('show')
    })
    $('body').on('click', '#btn_chitiet', function () {
        let id = $(this).attr('data-id')
        $.get('/Detail-coupon/' + id, function (re) {
            if (re.status) {
                $('#pp_modal').modal('show');
                setdata(re.data);
            }
        })
    })
    $('body').on('click', '#btn_xoa', function () {

    })
    $('body').on('click', '#btn_save', function () {
        let id = $('#btn_ID').val();
        var send = getData();
        if ($('#btn_ID').val() == null || $('#btn_ID').val() == '' || $('#btn_ID').val() == undefined)
        {
            $.post('/add-Coupon', { bk: send }, function (re) {
                if (re.status) {
                    alert(re.message);
                    $('#pp_modal').modal('hide');
                    window.location.reload();
                }
                else {
                    alert(re.message)
                }
            })
        }
        else
        {
            $.post('/Update-coupon/'+id, { vc: send }, function (re) {
                if (re.status) {
                    alert(re.message);
                    $('#pp_modal').modal('hide');
                    window.location.reload();
                }
                else {
                    alert(re.message)
                }
            })
        }
    })

})
function setdata(data) {
    if (data == null || data == undefined || data== '') {
        $('#btn_ID').val('');
        $('#btn_name').val('');
        $('#btn_PercentDiscount').val('');
        $('#btn_StartDate').val('');
        $('#btn_EndDate').val('');
        $('#btn_Description').val('');
        $('#btn_Status').val('');
    }
    else {
        $('#btn_ID').val(data.couponID);
        $('#btn_name').val(data.couponName);
        $('#btn_PercentDiscount').val(data.percentDiscount);
        $('#btn_StartDate').val(data.startDate);
        $('#btn_EndDate').val(data.endDate);
        $('#btn_Description').val(data.description);
        $('#btn_Status').val(data.status);
    }
}
function getData() {
    return {

    CouponID       :  $('#btn_ID').val(),
    CouponName        :  $('#btn_name').val(),
    PercentDiscount      :  $('#btn_PercentDiscount').val(),
    StartDate     :  $('#btn_StartDate').val(),
    EndDate       :  $('#btn_EndDate').val(),
    Description    :  $('#btn_Description').val(),
    Status         :  $('#btn_Status').val(),
    }
}