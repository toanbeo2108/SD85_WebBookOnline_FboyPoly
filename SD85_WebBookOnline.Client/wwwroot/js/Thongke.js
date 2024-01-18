

$(document).ready(function () {
   
    InitPage()
    InitComBo()
    $('#tittle').text('Thống kê doanh thu sách bán lẻ')

    $('body').on('click', '#btn_loc', function () {
        if ($('#ft_TUNGAY_tu').val() == '' &&
            $('#ft_kethuc_den').val() == '' &&
            $('#btn_search').val() == ''
        ) {
            alert('Bạn cần ít nhất 1 điều kiện')
            return;

        }

        var ngay_tu = '';
        var ngay_den = '';
        var ten_sach = '';
        var ngaytu = moment($('#ft_TUNGAY_tu').val()).format('DD-MM-YYYY');
        var ngayden = moment($('#ft_kethuc_den').val()).format('DD-MM-YYYY');
        var tensach = $('#btn_search').val();
        if ($('#btn_search').val() != '') {
            ten_sach = (' sách ' + tensach);
        }
        if ($('#ft_TUNGAY_tu').val() != '') {
            ngay_tu = (' từ ngày ' + ngaytu);
        }
        if ($('#ft_kethuc_den').val() != '') {
            ngay_den = (' đến ngày ' + ngayden);
        }
        InitPage3();
        $('#tittle').text('Thống kê doanh thu bán lẻ ' + ten_sach + ngay_tu + ngay_den)
        $('#thongke-options').val('')
        $('#modal-filter').modal('hide');

    })
    $('body').on('change', '#thongke-options', function () {

        if ($('#thongke-options').val() == 1) {
            $('#tittle').text('Thống kê tổng doanh thu sách bán lẻ')
            
            InitPage()
        }
        if ($('#thongke-options').val() == 2) {
            $('#tittle').text('Thống kê doanh thu bán lẻ ngày hôm nay')
            InitPage2()
        }
    });



    $('#btn-openfilter').on('click', function () {
        $('#ft_TUNGAY_tu').val(moment().format('YYYY-MM-DD'));

        $('#ft_kethuc_den').val(moment().format('YYYY-MM-DD'));
        $('#btn_search').val('');
        $('#modal-filter').modal('show')
    })
    $('#btn_exit').on('click', function () {

        $('#modal-filter').modal('hide')
    })
    $('body').on('click', '#btn_clear', function () {
        $('#ft_TUNGAY_tu').val(moment().format('YYYY-MM-DD'));

        $('#ft_kethuc_den').val(moment().format('YYYY-MM-DD'));
        $('#btn_search').val('');
    })
})

function InitPage() {
    $.get('/GetThongKe', function (re) {
        if (re.status) {
            // Hủy DataTable hiện tại (nếu có)
            if ($.fn.DataTable.isDataTable('#table-thongke')) {
                $('#table-thongke').DataTable().destroy();
            }

            let header = `
                <thead>
                    <tr>
                        <th>Tên sách</th>
                        <th>Tổng số sách bán được</th>
                        <th>Tổng doanh thu</th>
                        <th>Lợi nhuận</th>
                        <th>Chi phí gốc</th>
                        <th>Số sách còn lại</th>
                    </tr>
                </thead>`;
            let body = '';

            // Log dữ liệu từ server
            console.log("Dữ liệu từ server:", re.data);

            if (re.data.length > 0) {
                re.data.forEach(row => {
                    let tr = `<tr>
                                <td>${row.tensach}</td>
                                <td>${row.tongSoSachBanDuoc}</td>
                                <td>${row.tongDoanhThusach}</td>
                                <td>${row.loiNhuansach}</td>
                                <td>${row.chiPhiGocsach}</td>
                                <td>${row.soSachConLai != 0 ? row.soSachConLai : ''}</td>
                              </tr>`;
                    body += tr;

                });
            }

            // Thêm table vào HTML
            $('#table-thongke').html(header + body);

            // Khởi tạo DataTable
            $('#table-thongke').DataTable({
                "pageLength": 5,
                "searching": false
            });

            
        } else {
            alert(re.message);
        }
    });
}




function getBindFormFilter() {
    return {
        NTTU_tu: $('#ft_TUNGAY_tu').val() != '' ? $('#ft_TUNGAY_tu').val() : '',
        NTDEN_den: $('#ft_kethuc_den').val() != '' ? $('#ft_kethuc_den').val() : '',
        _search: $('#btn_search').val(),
    }
}

function InitPage2() {
    $.get('/ThongkeNgay', function (re) {
        if (re.status) {
            // Hủy DataTable hiện tại (nếu có)
            if ($.fn.DataTable.isDataTable('#table-thongke')) {
                $('#table-thongke').DataTable().destroy();
            }

            let header = `
                <thead>
                    <tr>
                        <th>Tên sách</th>
                        <th>Tổng số sách bán được</th>
                        <th>Tổng doanh thu</th>
                        <th>Lợi nhuận</th>
                        <th>Chi phí gốc</th>
                        <th>Số sách còn lại</th>
                    </tr>
                </thead>`;
            let body = '';

            // Log dữ liệu từ server
            console.log("Dữ liệu từ server:", re.data);

            if (re.data.length > 0) {
                re.data.forEach(row => {
                    let tr = `<tr>
                                <td>${row.tensach}</td>
                                <td>${row.tongSoSachBanDuoc}</td>
                                <td>${row.tongDoanhThusach}</td>
                                <td>${row.loiNhuansach}</td>
                                <td>${row.chiPhiGocsach}</td>
                                <td>${row.soSachConLai != 0 ? row.soSachConLai : ''}</td>
                              </tr>`;
                    body += tr;
                });
            }

            // Thêm table vào HTML
            $('#table-thongke').html(header + body);

            // Khởi tạo DataTable
            $('#table-thongke').DataTable({
                "pageLength": 4,
                "searching": false
            });

            
        } else {
            alert(re.message);
        }
    });

}
function InitPage3() {
    $.post('/load-data-thongke', { filter: getBindFormFilter() }, function (re) {
        if (re.status) {
            // Hủy DataTable hiện tại (nếu có)
            if ($.fn.DataTable.isDataTable('#table-thongke')) {
                $('#table-thongke').DataTable().destroy();
            }

            let header = `
                <thead>
                    <tr>
                        <th>Tên sách</th>
                        <th>Tổng số sách bán được</th>
                        <th>Tổng doanh thu</th>
                        <th>Lợi nhuận</th>
                        <th>Chi phí gốc</th>
                        <th>Số sách còn lại</th>
                    </tr>
                </thead>`;
            let body = '';

            // Log dữ liệu từ server
            console.log("Dữ liệu từ server:", re.data);

            if (re.data.length > 0) {
                re.data.forEach(row => {
                    let tr = `<tr>
                                <td>${row.tensach}</td>
                                <td>${row.tongSoSachBanDuoc}</td>
                                <td>${row.tongDoanhThusach}</td>
                                <td>${row.loiNhuansach}</td>
                                <td>${row.chiPhiGocsach}</td>
                                <td>${row.soSachConLai != 0 ? row.soSachConLai : ''}</td>
                              </tr>`;
                    body += tr;
                });
            }

            // Thêm table vào HTML
            $('#table-thongke').html(header + body);

            // Khởi tạo DataTable
            $('#table-thongke').DataTable({
                "pageLength": 4,
                "searching": false
            });

           
        } else {
            alert(re.message);
        }
    });
}

function InitComBo() {
    $('#tittlecb').text('Thống kê tổng doanh thu Combo')
    $.get('/GetComBo', function (re) {
        if (re.status) {
            // Hủy DataTable hiện tại (nếu có)
            if ($.fn.DataTable.isDataTable('#table-Combo')) {
                $('#table-thongke').DataTable().destroy();
            }

            let header = `
                <thead>
                    <tr>
                        <th>Tên sách</th>
                        <th>Tổng số sách ComBo được</th>
                        <th>Tổng doanh thu</th>
                        
                    </tr>
                </thead>`;
            let body = '';

            // Log dữ liệu từ server
            console.log("Dữ liệu từ server:", re.data);

            if (re.data.length > 0) {
                re.data.forEach(row => {
                    let tr = `<tr>
                                <td>${row.tensach}</td>
                                <td>${row.tongSoSachBanDuoc}</td>
                                <td>${row.tongDoanhThusach}</td>
                               
                              </tr>`;
                    body += tr;

                });
            }

            // Thêm table vào HTML
            $('#table-Combo').html(header + body);

            // Khởi tạo DataTable
            $('#table-Combo').DataTable({
                "pageLength": 5,
                "searching": false
            });


        } else {
            alert(re.message);
        }
    });
}



function generateExcel() {
    if ($('#thongke-options').val() == 1) {
        $.get('/GetThongKe', function (re) {
            if (re.status) {
                var data = re.data;
                excel(data)
                // Tạo một đối tượng JSON để lưu trữ dữ liệu

            } else {
                alert(re.message);
            }
        });
    }
    if ($('#thongke-options').val() == 2) {
        $.get('/ThongkeNgay', function (re) {
            if (re.status) {
                var data = re.data;
                excel(data)
                // Tạo một đối tượng JSON để lưu trữ dữ liệu

            } else {
                alert(re.message);
            }
        });
    }
    if ($('#thongke-options').val() == null) {
        $.post('/load-data-thongke', { filter: getBindFormFilter() }, function (re) {
            if (re.status) {
                var data = re.data;
                excel(data)
                // Tạo một đối tượng JSON để lưu trữ dữ liệu

            } else {
                alert(re.message);
            }
        });
    }
}


function excel(data) {
    var workbook = new ExcelJS.Workbook();
    var worksheet = workbook.addWorksheet('ThongKeSheet');
    var headerRow = worksheet.addRow([
        'Tên sách',
        'Tổng số sách bán được',
        'Tổng doanh thu',
        'Lợi nhuận',
        'Chi phí gốc',
        'Số sách còn lại'
    ]);

    for (var i = 1; i <= 6; i++) {
        headerRow.getCell(i).fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FF008000' }
        };
    }

    data.forEach(function (item) {
        var dataRow = worksheet.addRow([
            item.tensach,
            item.tongSoSachBanDuoc,
            item.tongDoanhThusach,
            item.loiNhuansach,
            item.chiPhiGocsach,
            item.soSachConLai != 0 ? item.soSachConLai : ''
        ]);
    });

    workbook.xlsx.writeBuffer().then(buffer => {
        var blob = new Blob([buffer], { type: 'application/octet-stream' });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = 'ThongKe.xlsx';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    });
}
