class DataTableHandler {
    constructor(tableId, jsonData) {
        this.tableId = tableId;
        this.columns = jsonData.columns;
        this.data = jsonData.data;
    }

    initializeTable() {
        this.createHeaders();
        this.populateTable();
    }

    createHeaders() {
        this.columns.forEach(column => {
            $(`${this.tableId} thead tr`).append(`<th>${column.title}</th>`);
        });
    }

    populateTable() {
        $(this.tableId).DataTable({
            data: this.data,
            columns: this.columns,
            fixedHeader: true,
            responsive: true,
            scrollCollapse: true,
            scrollY: '50vh',
            scrollX: true,
            deferRender: true,
            scroller: true,
            autoWidth: true,

            layout: {
                topStart: {
                    buttons: [{
                        extend: 'copyHtml5',
                        className: 'btn btn-outline-dark btn-sm'
                    },
                    {
                        extend: 'csv',
                        className: 'btn btn-outline-dark btn-sm'
                    },
                    {
                        extend: 'excel',
                        className: 'btn btn-outline-dark btn-sm'
                    },
                    {
                        extend: 'pdf',
                        className: 'btn btn-outline-dark btn-sm'
                    },
                    {
                        extend: 'print',
                        className: 'btn btn-outline-dark btn-sm'
                    }]
                },
                bottom: ['pageLength', 'paging'],
                bottomStart: null,
                bottomEnd: null,
            }
        });
        $(window).resize(() => {
            setTimeout(function () {
                $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            }, 200);
        });
    }
}

$(document).ready(function () {
    debugger
    const jsonData = {
        columns: [
            { title: "Sr No.", data: "srNo" },
            { title: "Agency Name", data: "agencyName" },
            { title: "Contact No.", data: "contactNo" },
            { title: "Email", data: "email" },
            { title: "GST No", data: "gstNo" },
            { title: "PAN No", data: "panNo" },
            { title: 'View Dropdown', data: 'view' },
            { title: "Action", data: "action" }
        ],
        data: [
            {
                srNo: 1,
                agencyName: "HELLO",
                contactNo: "9898966985",
                email: "pratul@amvex.com",
                gstNo: "GST789456123056",
                panNo: "PAN123456",
                view: '<div class="btn-group" role="group"><button id="btnGroupDrop1" type="button" class="btn btn-primary dropdown-toggle btn-sm" data-bs-toggle="dropdown" aria-expanded="false">Dropdown</button><ul class="dropdown-menu" aria-labelledby="btnGroupDrop1"><li><a class="dropdown-item" href="#">Dropdown link</a></li><li><a class="dropdown-item" href="#">Dropdown link</a></li></ul></div>',
                action: "<button class='btn btn-primary btn-sm'>Edit</button> <button class='btn btn-danger btn-sm' data-bs-toggle='modal' data-bs-target='#crudModal' data-bs-whatever='mdo'>Delete</button>"
            },
            {
                srNo: 2,
                agencyName: "HELLO",
                contactNo: "9898966985",
                email: "pratul@amvex.com",
                gstNo: "GST789456123056",
                panNo: "PAN123456",
                view: '<div class="btn-group" role="group"><button id="btnGroupDrop1" type="button" class="btn btn-primary dropdown-toggle btn-sm" data-bs-toggle="dropdown" aria-expanded="false">Dropdown</button><ul class="dropdown-menu" aria-labelledby="btnGroupDrop1"><li><a class="dropdown-item" href="#">Dropdown link</a></li><li><a class="dropdown-item" href="#">Dropdown link</a></li></ul></div>',
                action: "<button class='btn btn-primary btn-sm'>Edit</button> <button class='btn btn-danger btn-sm btn-delete'>Delete</button>"
            },
            {
                srNo: 3,
                agencyName: "HELLO",
                contactNo: "9898966985",
                email: "pratul@amvex.com",
                gstNo: "GST789456123056",
                panNo: "PAN123456",
                view: '<div class="btn-group" role="group"><button id="btnGroupDrop1" type="button" class="btn btn-primary dropdown-toggle btn-sm" data-bs-toggle="dropdown" aria-expanded="false">Dropdown</button><ul class="dropdown-menu" aria-labelledby="btnGroupDrop1"><li><a class="dropdown-item" href="#">Dropdown link</a></li><li><a class="dropdown-item" href="#">Dropdown link</a></li></ul></div>',
                action: "<button class='btn btn-primary btn-sm'>Edit</button> <button class='btn btn-danger btn-sm btn-delete'>Delete</button>"
            },
            {
                srNo: 4,
                agencyName: "HELLO",
                contactNo: "9898966985",
                email: "pratul@amvex.com",
                gstNo: "GST789456123056",
                panNo: "PAN123456",
                view: '<div class="btn-group" role="group"><button id="btnGroupDrop1" type="button" class="btn btn-primary dropdown-toggle btn-sm" data-bs-toggle="dropdown" aria-expanded="false">Dropdown</button><ul class="dropdown-menu" aria-labelledby="btnGroupDrop1"><li><a class="dropdown-item" href="#">Dropdown link</a></li><li><a class="dropdown-item" href="#">Dropdown link</a></li></ul></div>',
                action: "<button class='btn btn-primary btn-sm'>Edit</button> <button class='btn btn-danger btn-sm btn-delete'>Delete</button>"
            },
            {
                srNo: 5,
                agencyName: "HELLO",
                contactNo: "9898966985",
                email: "pratul@amvex.com",
                gstNo: "GST789456123056",
                panNo: "PAN123456",
                view: '<div class="btn-group" role="group"><button id="btnGroupDrop1" type="button" class="btn btn-primary dropdown-toggle btn-sm" data-bs-toggle="dropdown" aria-expanded="false">Dropdown</button><ul class="dropdown-menu" aria-labelledby="btnGroupDrop1"><li><a class="dropdown-item" href="#">Dropdown link</a></li><li><a class="dropdown-item" href="#">Dropdown link</a></li></ul></div>',
                action: "<button class='btn btn-primary btn-sm'>Edit</button> <button class='btn btn-danger btn-sm btn-delete'>Delete</button>"
            },
            {
                srNo: 6,
                agencyName: "HELLO",
                contactNo: "9898966985",
                email: "pratul@amvex.com",
                gstNo: "GST789456123056",
                panNo: "PAN123456",
                view: '<div class="btn-group" role="group"><button id="btnGroupDrop1" type="button" class="btn btn-primary dropdown-toggle btn-sm" data-bs-toggle="dropdown" aria-expanded="false">Dropdown</button><ul class="dropdown-menu" aria-labelledby="btnGroupDrop1"><li><a class="dropdown-item" href="#">Dropdown link</a></li><li><a class="dropdown-item" href="#">Dropdown link</a></li></ul></div>',
                action: "<button class='btn btn-primary btn-sm'>Edit</button> <button class='btn btn-danger btn-sm btn-delete'>Delete</button>"
            },
            {
                srNo: 7,
                agencyName: "HELLO",
                contactNo: "9898966985",
                email: "pratul@amvex.com",
                gstNo: "GST789456123056",
                panNo: "PAN123456",
                view: '<div class="btn-group" role="group"><button id="btnGroupDrop1" type="button" class="btn btn-primary dropdown-toggle btn-sm" data-bs-toggle="dropdown" aria-expanded="false">Dropdown</button><ul class="dropdown-menu" aria-labelledby="btnGroupDrop1"><li><a class="dropdown-item" href="#">Dropdown link</a></li><li><a class="dropdown-item" href="#">Dropdown link</a></li></ul></div>',
                action: "<button class='btn btn-primary btn-sm'>Edit</button> <button class='btn btn-danger btn-sm btn-delete'>Delete</button>"
            },
            {
                srNo: 8,
                agencyName: "HELLO",
                contactNo: "9898966985",
                email: "pratul@amvex.com",
                gstNo: "GST789456123056",
                panNo: "PAN123456",
                view: '<div class="btn-group" role="group"><button id="btnGroupDrop1" type="button" class="btn btn-primary dropdown-toggle btn-sm" data-bs-toggle="dropdown" aria-expanded="false">Dropdown</button><ul class="dropdown-menu" aria-labelledby="btnGroupDrop1"><li><a class="dropdown-item" href="#">Dropdown link</a></li><li><a class="dropdown-item" href="#">Dropdown link</a></li></ul></div>',
                action: "<button class='btn btn-primary btn-sm'>Edit</button> <button class='btn btn-danger btn-sm btn-delete'>Delete</button>"
            }, {
                srNo: 9,
                agencyName: "HELLO",
                contactNo: "9898966985",
                email: "pratul@amvex.com",
                gstNo: "GST789456123056",
                panNo: "PAN123456",
                view: '<div class="btn-group" role="group"><button id="btnGroupDrop1" type="button" class="btn btn-primary dropdown-toggle btn-sm" data-bs-toggle="dropdown" aria-expanded="false">Dropdown</button><ul class="dropdown-menu" aria-labelledby="btnGroupDrop1"><li><a class="dropdown-item" href="#">Dropdown link</a></li><li><a class="dropdown-item" href="#">Dropdown link</a></li></ul></div>',
                action: "<button class='btn btn-primary btn-sm'>Edit</button> <button class='btn btn-danger btn-sm btn-delete'>Delete</button>"
            }, {
                srNo: 10,
                agencyName: "HELLO",
                contactNo: "9898966985",
                email: "pratul@amvex.com",
                gstNo: "GST789456123056",
                panNo: "PAN123456",
                view: '<div class="btn-group" role="group"><button id="btnGroupDrop1" type="button" class="btn btn-primary dropdown-toggle btn-sm" data-bs-toggle="dropdown" aria-expanded="false">Dropdown</button><ul class="dropdown-menu" aria-labelledby="btnGroupDrop1"><li><a class="dropdown-item" href="#">Dropdown link</a></li><li><a class="dropdown-item" href="#">Dropdown link</a></li></ul></div>',
                action: "<button class='btn btn-primary btn-sm'>Edit</button> <button class='btn btn-danger btn-sm btn-delete'>Delete</button>"
            }
            // Add more data objects here
        ]
    };

    /*setTimeout(function () {*/
    const dataTableHandler = new DataTableHandler('#example', jsonData);
    dataTableHandler.initializeTable();


    $('.btn-delete').on("click", function () {
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'error',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                // Perform the delete action here
                // For example, you can make an AJAX call to delete the data
                Swal.fire(
                    'Deleted!',
                    'Your row has been deleted.',
                    'success'
                );
            }
        });
    });
    /*},0);*/

    $('#js-filtertoggle').on('click', function () {
        $('.slideright').toggleClass('active');
    });
});