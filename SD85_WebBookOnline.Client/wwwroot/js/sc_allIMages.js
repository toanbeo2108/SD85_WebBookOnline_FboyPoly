$(document).ready(function () {
    ///



    $('body').on('click', '#themmoi', function () {
        setData('');
        $('#exampleModal').modal('show');
        
    })

    $('body').on('click', '#xemchitiet', function (re) {
        let id = $(this).attr('data-id');
        
        $.get('/detail-image/' + id,function (re) {
            if (re.status) {
                setData(re.data)
                  $('#exampleModal').modal('show');

            }
            else {
                alert(re.message)
            }
        })
        
        

    })
    
    $('body').on('click', '#xoa', function () {
        let id = $(this).attr('data-id');
        
        $.get('/delete-image/' + id , function (re) {
            if (re.status) {
                alert(re.message)
                window.location.reload();
            }
            else {
                alert(re.message)
            }
        })
        
    })

    $('body').on('click', '#btn-them', function () {
        var send = getData();    
        
        if ($('#btn_imgID').val() == null || $('#btn_imgID').val() == '' || $('#btn_imgID').val() == undefined)
        {
            $.post('/themm-image', { img: send }, function (re) {
                if (re.status) {

                    alert(re.message);
                    $('#exampleModal').modal('hide');

                    window.location.reload();
                }
                else {
                    alert(re.message);

                }
            })
        }
        else {
            let id = $('#btn_imgID').val();
            $.post('/update-img/'+id, { img: send }, function (re) {
                
                if (re.status) {

                    alert(re.message);
                    $('#exampleModal').modal('hide');

                    window.location.reload();
                }
                else {
                    alert(re.message);

                }
            })
        } 
              
    });
})

    function setData(data) {

        if (data != null && data != undefined && data!= "") {
           
            $('#btn_imgID').val(data.imagesID);
            $('#cb_bookname').val(data.bookID);
            $('#btn_imgname').val(data.imageName);
            $('#btn_stt').val(data.status);

        } else {
            
            $('#btn_imgID').val('');
            $('#cb_bookname').val('');
            $('#btn_imgname').val('');
            $('#btn_stt').val('');
        }
    }

function getData() {
    return {
        ImagesID: $('#btn_imgID').val(),
        BookID: $('#cb_bookname').val(),
        ImageName: $('#btn_imgname').val(),
        Status: $('#btn_stt').val()
    }
}
