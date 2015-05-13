
$(document).ready(function () {

    var oTable = $('#myDataTable').dataTable({
    	"bServerSide": true,
    	"sAjaxSource": "AjaxHandler",
    	"bProcessing": true,
        "bRetrieve":true,
    	"aoColumns": [
            {   "sName": "Func",
                            "bSearchable": false,
                            "bSortable": false,
                            "fnRender": function (oObj) {
                                return ' <a href=\"Edit/' + oObj.aData[0] + '\">Edit</a> |' +
                                        '<a href=\"Details/' + oObj.aData[0] + '\">Details</a> |' +
                                       ' <a href=\"Delete/' + oObj.aData[0] + '\">Delete</a> ';
                            }
            },
            { "sName": "ID" },
			{ "sName": "Name" },
            { "sName": "Descr" }
		            ]
    });
});
