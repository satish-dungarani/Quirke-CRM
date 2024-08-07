class DataTableHandler {
    constructor(tableId, ajaxUrl) {
        this.tableId = tableId;
        this.ajaxUrl = ajaxUrl;
    }

    initializeTable() {
        this.populateTable();
    }

    createHeaders() {
       
        columnHeaders.forEach(title => {
            $(`${this.tableId} thead tr`).append(`<th>${title}</th>`);
        });
    }

    populateTable() {
        $(this.tableId).DataTable({
            ajax: {
                url: this.ajaxUrl,
                type: "GET",
                datatype: "json",
                dataSrc: ""
            },
            columns: [
                { "data": "Id" },
                { "data": "Firstname" },
                { "data": "Lastname" },
                { "data": "BirtDate" },
                { "data": "Gender" },
                { "data": "Mobile" },
                { "data": "Email" },
                { "data": "CreatedOn" }
            ],
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
