function handleSaveLayout() {
	var e = $(".demo").html();
	if (e != window.demoHtml) {
		saveLayout();
		window.demoHtml = e
	}
}
var stopsave;
var startdrag;
function supportstorage() {
	if (typeof window.localStorage == 'object')
		return true;
	else
		return false;
}
var layouthistory;
function handleJsIds() {
	handleModalIds();
	handleAccordionIds();
	handleCarouselIds();
	handleTabsIds();
	handleDateTimes();
	handleDyTable();
	handleDySelect();
}
function handleAccordionIds() {
	var e = $(".demo #myAccordion");
	var t = randomNumber();
	var n = "panel-" + t;
	var r;
	e.attr("id", n);
	e.find(".panel").each(function (e, t) {
		r = "panel-element-" + randomNumber();
		$(t).find(".panel-title").each(function (e, t) {
			$(t).attr("data-parent", "#" + n);
			$(t).attr("href", "#" + r)
		});
		$(t).find(".panel-collapse").each(function (e, t) {
			$(t).attr("id", r)
		})
	})
}
function handleCarouselIds() {
	var e = $(".demo #myCarousel");
	var t = randomNumber();
	var n = "carousel-" + t;
	e.attr("id", n);
	e.find(".carousel-indicators li").each(function (e, t) {
		$(t).attr("data-target", "#" + n)
	});
	e.find(".left").attr("href", "#" + n);
	e.find(".right").attr("href", "#" + n)
}
function handleModalIds() {
	var e = $(".demo #myModalLink");
	var t = randomNumber();
	var n = "modal-container-" + t;
	var r = "modal-" + t;
	e.attr("id", r);
	e.attr("href", "#" + n);
	e.next().attr("id", n)
}
function handleDateTimes() {
	$('.form_datetime').datetimepicker({
		language:  'zh-CN',
		weekStart: 1,
		todayBtn: 1,
		autoclose: 1,
		todayHighlight: 1,
		startView: 2,
		forceParse: 0,
		showMeridian: 1
	});
	$('.form_date').datetimepicker({
		language: 'zh-CN',
		weekStart: 1,
		todayBtn: 1,
		autoclose: 1,
		todayHighlight: 1,
		startView: 2,
		minView: 2,
		forceParse: 0
	});
	$('.form_time').datetimepicker({
		language: 'zh-CN',
		weekStart: 1,
		todayBtn: 1,
		autoclose: 1,
		todayHighlight: 1,
		startView: 1,
		minView: 0,
		maxView: 1,
		forceParse: 0
	});
}
function handleTabsIds() {
	var e = $(".demo #myTabs");
	var t = randomNumber();
	var n = "tabs-" + t;
	e.attr("id", n);
	e.find(".tab-pane").each(function (e, t) {
		var n = $(t).attr("id");
		var r = "panel-" + randomNumber();
		$(t).attr("id", r);
		$(t).parent().parent().find("a[href=#" + n + "]").attr("href", "#" + r)
	})
}

function handleDyTable() {

	var e = $(".demo #dytable");
	if (e != undefined && e != null && e.length > 0) {


		var t = randomNumber();
		var n = "table-" + t;
		var toolid = "toolbar-" + t;
		var scriptid = "tablejson-" + t;
		e.attr("id", n);
		var tool = e.parent("div").find("div[data-filter='dynamictable_toolbar']");
		tool.attr("id", toolid);
		var scriptobj = e.parent("div").find("script[tablejson='true']");
		scriptobj.attr("id", scriptid);

	 
	   $('#' + n).bootstrapTable({
			height: 550,
			locale: "zh-CN",
			toolbar: toolid,
			ID: n,
			columns: [[
				{
					field: 'state',
					checkbox: true,
					title: "ID",
					align: 'center',
					valign: 'middle'
				},
				{
					field: 'c1',

					title: "???1",
					align: 'center',
					valign: 'middle'
				},
				{
					field: 'c2',

					title: "???3",
					align: 'center',
					valign: 'middle'
				}
				,
				{
					field: 'c3',

					title: "???3",
					align: 'center',
					valign: 'middle'
				}
				,
				{
					field: 'c4',

					title: "???4",
					align: 'center',
					valign: 'middle'
				}
				,
				{
					field: 'c5',

					title: "???5",
					align: 'center',
					valign: 'middle'
				}
				,
				{
					field: 'c6',

					title: "???6",
					align: 'center',
					valign: 'middle'
				}
				,
				{
					field: 'c7',

					title: "???7",
					align: 'center',
					valign: 'middle'
				}
				,
				{
					field: 'c8',

					title: "???8",
					align: 'center',
					valign: 'middle'
				}

			]]
		})
	}
}
function handleDySelect() {
	var e = $(".demo #dyselect");
	if (e != undefined && e != null && e.length > 0) {
		var t = randomNumber();
		var n = "select-" + t;
	 
		var scriptid = "selectjson-" + t;
		e.attr("id", n);
	 
		var scriptobj = e.parent("div").find("script[selectjson='true']");
		scriptobj.attr("id", scriptid);


	}

}
function SetContentEdit() {
	CKEDITOR.disableAutoInline = true;

	var contenthandle = CKEDITOR.replace('contenteditor', {
		language: 'zh-cn',
		contentsCss: ['/Content/css/bootstrap.min.css'],
		allowedContent: true
	});
	 
}
function randomNumber() {
	return randomFromInterval(1, 1e6)
}
function randomFromInterval(e, t) {
	return Math.floor(Math.random() * (t - e + 1) + e)
}
function gridSystemGenerator() {
	$(".lyrow .preview input").bind("keyup",
		function () {
			var e = 0;
			var t = "";
			var n = false;
			var r = $(this).val().split(" ", 12);
			$.each(r,
				function (r, i) {
					if (!n) {
						if (parseInt(i) <= 0) n = true;
						e = e + parseInt(i);
						t += '<div class="col-md-' + i + ' column"></div>'
					}
				});
			if (e == 12 && !n) {
				$(this).parent().next().children().html(t);
				$(this).parent().prev().show()
			} else {
				$(this).parent().prev().hide()
			}
		})
}
function configurationElm(e, t) {
	$(".demo").delegate(".configuration > a", "click",
		function (e) {
			e.preventDefault();
			var t = $(this).parent().next().next().children();
			$(this).toggleClass("active");
			t.toggleClass($(this).attr("rel"))
		});
	$(".demo").delegate(".configuration .dropdown-menu a", "click",
		function (e) {
			e.preventDefault();
			var t = $(this).parent().parent();
			var n = t.parent().parent().next().next().children();
			t.find("li").removeClass("active");
			$(this).parent().addClass("active");
			var r = "";
			t.find("a").each(function () {
				r += $(this).attr("rel") + " "
			});
			t.parent().removeClass("open");
			n.removeClass(r);
			n.addClass($(this).attr("rel"))
		})
}
function removeElm() {
	$(".demo").delegate(".remove", "click",
		function (e) {
			e.preventDefault();
			$(this).parent().remove();
			if (!$(".demo .lyrow").length > 0) {
				clearDemo()
			}
		})
}
function clearDemo() {
	$(".demo").empty()
}
function removeMenuClasses() {
	$("#menu-layoutit li button").removeClass("active")
}
function changeStructure(e, t) {
	$("#download-layout ." + e).removeClass(e).addClass(t)
}
function cleanHtml(e) {
	$(e).parent().append($(e).children().html())
}
function downloadLayoutSrc() {
	var e = "";
	$("#download-layout").children().html($(".demo").html());
	var t = $("#download-layout").children();
	t.find(".preview, .configuration, .drag, .remove").remove();
	t.find(".lyrow").addClass("removeClean");
	t.find(".box-element").addClass("removeClean");
	t.find(".lyrow .lyrow .lyrow .lyrow .lyrow .removeClean").each(function () {
		cleanHtml(this)
	});
	t.find(".lyrow .lyrow .lyrow .lyrow .removeClean").each(function () {
		cleanHtml(this)
	});
	t.find(".lyrow .lyrow .lyrow .removeClean").each(function () {
		cleanHtml(this)
	});
	t.find(".lyrow .lyrow .removeClean").each(function () {
		cleanHtml(this)
	});
	t.find(".lyrow .removeClean").each(function () {
		cleanHtml(this)
	});
	t.find(".removeClean").each(function () {
		cleanHtml(this)
	});
	t.find(".removeClean").remove();
	$("#download-layout .column").removeClass("ui-sortable");
	$("#download-layout .row-fluid").removeClass("clearfix").children().removeClass("column");
	if ($("#download-layout .container").length > 0) {
		changeStructure("row-fluid", "row")
	}
	
	//formatSrc = $.htmlClean($("#download-layout").html(), {
	//	format: true,
	//	allowedAttributes: [["id"], ["class"], ["data-toggle"], ["data-target"], ["data-parent"], ["role"], ["data-dismiss"], ["aria-labelledby"], ["aria-hidden"], ["data-slide-to"], ["data-slide"]]
	//});
	formatSrc = $("#download-layout").html();
	$("#download-layout").html(formatSrc);
	$("#downloadModal textarea").empty();
	$("#downloadModal textarea").val(formatSrc)
}
var currentDocument = null;
var timerSave = 2e3;
var demoHtml = $(".demo").html();
$(window).resize(function () {
	$("body").css("min-height", $(window).height() - 90);
	$(".demo").css("min-height", $(window).height() - 160)
});
$(document).on('hidden.bs.modal', function (e) {
	$(e.target).removeData('bs.modal');
});


function documentready() {

	$(document).ready(function () {
		CKEDITOR.disableAutoInline = true;
		var contenthandle = CKEDITOR.replace('contenteditor', {
			language: 'zh-cn',
			contentsCss: ['/Content/css/bootstrap.min.css'],
			allowedContent: true
		});
		 
		restoreData();
		$("body").css("min-height", $(window).height() - 90);
		$(".demo").css("min-height", $(window).height() - 160);
		$(".demo, .demo .column").sortable({
			connectWith: ".column",
			opacity: .35,
			handle: ".drag"
		});
		$(".sidebar-nav .lyrow").draggable({
			connectToSortable: ".demo",
			helper: "clone",
			handle: ".drag",
			drag: function (e, t) {
				t.helper.width(400)
			},
			stop: function (e, t) {
				$(".demo .column").sortable({
					opacity: .35,
					connectWith: ".column"
				});
				handleJsIds();
				initContainer();
			}
		});
		$(".sidebar-nav .box").draggable({
			connectToSortable: ".column",
			helper: "clone",
			handle: ".drag",
			drag: function (e, t) {
				t.helper.width(400)
			},
			stop: function () {
				handleJsIds();
				initContainer();
			}
		});
		$("[data-target='#downloadModal']").click(function (e) {
			e.preventDefault();
			downloadLayoutSrc()
		});
		$('body.edit .demo').on("click", "[data-target='#editorModal']", function (e) {
			e.preventDefault();
			currenteditor = $(this).parent().parent().find('.view');
			if (currenteditor.length<=0)
			currenteditor = $(this).parent().parent().parent().find('.view');
			var eText = currenteditor.html();
			contenthandle.setData(eText);
		});
		$("#savecontent").click(function (e) {
			e.preventDefault();
			currenteditor.html(contenthandle.getData());
		});
		$("#edit").click(function () {
			$("body").removeClass("devpreview sourcepreview");
			$("body").addClass("edit");
			removeMenuClasses();
			$(this).addClass("active");
			return false
		});
		$("#clear").click(function (e) {
			e.preventDefault();
			clearDemo()
		});
		$("#devpreview").click(function () {
			$("body").removeClass("edit sourcepreview");
			$("body").addClass("devpreview");
			removeMenuClasses();
			$(this).addClass("active");
			return false
		});
		$("#sourcepreview").click(function () {
			$("body").removeClass("edit");
			$("body").addClass("devpreview sourcepreview");
			removeMenuClasses();
			$(this).addClass("active");
			return false
		});
		$(".nav-header").click(function () {
			$(".sidebar-nav .boxes, .sidebar-nav .rows").hide();
			$(this).next().slideDown()
		});
		$('#undo').click(function () {
			stopsave++;
			if (undoLayout()) initContainer();
			stopsave--;
		});
		$('#redo').click(function () {
			stopsave++;
			if (redoLayout()) initContainer();
			stopsave--;
		});
		$("#fluidPage").click(function (e) {
			e.preventDefault();
			changeStructure("container", "container-fluid");
			$("#fixedPage").removeClass("active");
			$(this).addClass("active");
			downloadLayoutSrc()
		});
		$("#fixedPage").click(function (e) {
			e.preventDefault();
			changeStructure("container-fluid", "container");
			$("#fluidPage").removeClass("active");
			$(this).addClass("active");
			downloadLayoutSrc()
		});
		
		removeElm();
		configurationElm();
		gridSystemGenerator();
		setInterval(function () {
			handleSaveLayout()
		},
			timerSave);
		initContainer();
		WebDesigner.InitDesigner();
	})
}


documentready();
function saveLayout() {
	var data = layouthistory;
	if (!data) {
		data = {};
		data.count = 0;
		data.list = [];

	}
	if (data.list.length > data.count) {
		for (i = data.count; i < data.list.length; i++)
			data.list[i] = null;
	}
	data.list[data.count] = window.demoHtml;
	data.count++;
	data.Id = $("#PageId").val();
	data.Title = $("#pageTitle").val();
	if (supportstorage()) {
		localStorage.setItem("layoutdata", JSON.stringify(data));
	}
	layouthistory = data;

}

 

function undoLayout() {
	var data = layouthistory;
	//console.log(data);
	if (data) {
		if (data.count < 2) return false;
		window.demoHtml = data.list[data.count - 2];
		data.count--;
		$('.demo').html(window.demoHtml);
		if (supportstorage()) {
			localStorage.setItem("layoutdata", JSON.stringify(data));
		}
		return true;
	}
	return false;
	/*$.ajax({  
		type: "POST",  
		url: "/build/getPreviousLayout",  
		data: { },  
		success: function(data) {
			undoOperation(data);
		}
	});*/
}
function redoLayout() {
	var data = layouthistory;
	if (data) {
		if (data.list[data.count]) {
			window.demoHtml = data.list[data.count];
			data.count++;
			$('.demo').html(window.demoHtml);
			if (supportstorage()) {
				localStorage.setItem("layoutdata", JSON.stringify(data));
			}
			return true;
		}
	}
	return false;

}

function restoreData() {
	if (supportstorage()) {
		var ldata = localStorage.getItem("layoutdata");
		if (ldata != null) {
			if (ldata == undefined || ldata == null || ldata.length == 0) {
				window.demoHtml = "";
				clearDemo();
			}
			else {
				if (!layouthistory) return false;
				layouthistory = JSON.parse(ldata);
				window.demoHtml = layouthistory.list[layouthistory.count - 1];
			}
			if (window.demoHtml) $(".demo").html(window.demoHtml);

			$("#PageId").val(ldata.Id);
			$("#pageTitle").val(ldata.Title);
		}
		else {
			clearDemo();
        }

	}
}
function restoreDataFromHtml(html) {
	if (html == "") {
		layouthistory = {};
		layouthistory.count = 0;
		layouthistory.list = [];
		window.demoHtml = "";
		clearDemo();
	}
	else {


		var ldata = JSON.parse(html);

		if (ldata == undefined || ldata == null || ldata.length == 0) {
			window.demoHtml = "";
			clearDemo();
		}
		else {
			layouthistory = {};
			layouthistory.count = 1;
			layouthistory.list = [];
			layouthistory.list.push(ldata);
			window.demoHtml = layouthistory.list[layouthistory.count - 1];
			if (window.demoHtml) $(".demo").html(window.demoHtml);
		}
	}


}



downloadLayoutSrc();
function restoreFromDbData(html) {
	if (html == null || html == "")
		html = "";

	restoreDataFromHtml(html);
	saveLayout();
}
function saveLayoutToDb() {
	var data = layouthistory;
	if (!data) {
		data = {};
		data.count = 0;
		data.list = [];
	}
	if (data.list.length > data.count) {
		for (i = data.count; i < data.list.length; i++)
			data.list[i] = null;
	}
	data.list[data.count] = window.demoHtml;
	data.count++;

	layouthistory = data;
	var html = downloadLayoutSrc();//??????????????????de html
	$.post("/Scada/ScaddDesigner/SavePage", { "Id": $("#PageId").val(), "LayoutData": JSON.stringify(data.list[data.count - 1]), "html": html }, function (result) {
		if (result.code == 0) {
			WebDesigner.showMessage("??????????????????");
			saveLayout();//???????????????
		}
		else {
			WebDesigner.showMessage("??????????????????");
		}

	});

}
function initContainer() {
	$(".demo, .demo .column").sortable({
		connectWith: ".column",
		opacity: .35,
		handle: ".drag",
		start: function (e, t) {
			if (!startdrag) stopsave++;
			startdrag = 1;
		},
		stop: function (e, t) {
			if (stopsave > 0) stopsave--;
			startdrag = 0;

		}
	});
	configurationElm();
	//????????????


}
function SCADA_WebDesigner() {
	//?????????????????????????????????
	this.LoadPages = function () {
		//??????????????????
		$.get("/Scada/ScaddDesigner/GetPages", null, function (result) {
			$("#mypage").empty();
			$("#mypage").append("<option value=''>?????????????????????</option>");
			for (var i = 0; i < result.data.length; i++) {

				$("#mypage").append("<option value='" + result.data[i].Id + "'>" + result.data[i].PageTitle + "</option>");
			}

		});
	}
	this.InitDesigner = function () {
		$("#PageId").val("0");
		WebDesigner.LoadPages();
		$("#btload").click(function (e) {

			if ($("#mypage").val() == "") {
				WebDesigner.showMessage("???????????????????????????");
			}
			else {
				$("#LoadPage").modal('hide');
				//??????????????????HTML
				var id = $("#mypage").val();
				$("#PageId").val(id);
				$.get("/Scada/ScaddDesigner/GetPage", { "id": id }, function (result) {
					var html = result.LayoutData;
					$("#pageTitle").html("???????????????????????????" + result.PageTitle);
					$("#jsFile").val(result.JsContent);
					$("#PageUid").val(result.PageUid);
					restoreFromDbData(html);
					initContainer();

				});
			}

		});

		$("#pagepreview").click(function (e) {
			e.preventDefault();
			var uid = $("#PageUid").val();
			window.open("../WebTemplate/" + uid + ".htm");

		});

		$("#btSaveJs").click(function (e) {
			e.preventDefault();
			var id = $("#PageId").val();
			$.post("/Scada/ScaddDesigner/SaveJsContent", { "Id": id, "JsContent": $("#jsFile").val() }, function (result) {

				if (result.code == 0) {
					WebDesigner.showMessage("??????????????????");
				}
				else {
					WebDesigner.showMessage("??????????????????");
				}
			});

		});
		//???????????????????????????
		//input text
		$('body.edit .demo').on("click", "[data-target='#inputTextFormModal']", function (e) {
			//??????????????????????????????
			e.preventDefault();
			protertieseditor = $(this).parent().parent().find('input[propertyeditable="true"]');
			if (protertieseditor.length <= 0)
				protertieseditor = $(this).parent().parent().parent().find('input[propertyeditable="true"]');
			$("#elementName").val(protertieseditor[0].id);
			$("#elementName").val(protertieseditor[0].name);
			$("#elementValue").val(protertieseditor.val());

		});

		$("#text_propertiessave").click(function (e) {
			//???????????????????????????
			e.preventDefault();
			protertieseditor.attr("id", $("#elementName").val());
			protertieseditor.attr("value", $("#elementValue").val());
		});
		//textarea
		$('body.edit .demo').on("click", "[data-target='#textareaFormModal']", function (e) {
			//??????????????????????????????
			e.preventDefault();
			protertieseditor = $(this).parent().parent().find('textarea[propertyeditable="true"]');
			if (protertieseditor.length <= 0)
				protertieseditor = $(this).parent().parent().parent().find('textarea[propertyeditable="true"]');
			$("#textarea_Name").val(protertieseditor[0].id);
			$("#textarea_Value").val(protertieseditor.val());
			$("#textarea_Number").val(protertieseditor.attr("rows"));
		});

		$("#textarea_propertiessave").click(function (e) {
			//???????????????????????????
			e.preventDefault();
			protertieseditor.attr("id", $("#textarea_Name").val());
			protertieseditor.attr("name", $("#textarea_Name").val());
			protertieseditor.text($("#textarea_Value").val());
			protertieseditor.attr("rows", $("#textarea_Number").val());

		});
		/////
		//textarea
		$('body.edit .demo').on("click", "[data-target='#inputRadioCheckBoxFormModal']", function (e) {
			//??????????????????????????????
			e.preventDefault();
			protertieseditor = $(this).parent().parent().find('input[propertyeditable="true"]');
			if (protertieseditor.length <= 0)
				protertieseditor = $(this).parent().parent().parent().find('input[propertyeditable="true"]');
			$("#radiocheckbox_Name").val(protertieseditor[0].id);
			$("#radiocheckbox_Status").attr("checked", protertieseditor.attr("checked"));

		});

		$("#radiocheckbox_propertiessave").click(function (e) {
			//???????????????????????????
			e.preventDefault();
			protertieseditor.attr("id", $("#radiocheckbox_Name").val());
			protertieseditor.attr("name", $("#radiocheckbox_Name").val());
			protertieseditor.attr("checked", $("#radiocheckbox_Status").attr("checked"));


		});
		$('body.edit .demo').on("click", "[data-target='#DynamicTableModal']", function (e) {
			//??????????????????
			e.preventDefault();
			protertieseditor = $(this).parent().parent().parent().find('table[propertyeditable="true"]');
			var jsJson = $(this).parent().parent().parent().find('script[tablejson="true"]');//??????json????????????
		
			var tableObj = JSON.parse(jsJson.html());
			if (tableObj != undefined && tableObj != null) {

				tableObj.ELE_ID = protertieseditor[0].id;
				var cnum = tableObj.ELE_ID.split('-')[1];
				var select1Id = "select1-" + cnum;
				var select2Id = "select2-" + cnum;
				var textkeyId = "text-" + cnum;
				var daterange1Id = "date1-" + cnum;
				var daterange2Id = "date1-" + cnum;
				tableObj.FILTER_SELECT1.ELE_ID = select1Id;
				tableObj.FILTER_SELECT2.ELE_ID = select2Id;
				tableObj.FILTER_KEY.ELE_ID = textkeyId;
				tableObj.FILTER_DATE_RANGE.ELE_ID = daterange1Id;
				tableObj.FILTER_DATE_RANGE.ELE2_ID = daterange2Id;

				$("#dynamictable_DataSource").val(tableObj.DATASOURCE);//???????????????
				$("#dynamictable_ID").val(tableObj.ELE_ID);
				$("#dynamictable_Title").val(tableObj.TABLE_TITLE);
				$("#dynamictable_Height").val(tableObj.TABLE_HEIGHT);
				$("#dynamictable_KeyId").val(tableObj.TABLE_KEY_ID_RECORD);
				if (tableObj.TABLE_SHOW_TOOL == true)
					$("#dynamictable_toolenable").attr("checked", true);
				else
					$("#dynamictable_toolenable").attr("checked", false);

				if (tableObj.TABLE_SHOW_PAGE == true)
					$("#dynamictable_pagenable").attr("checked", true);
				else
					$("#dynamictable_pagenable").attr("checked", false);

				$("#dynamictable_pagesize").val(tableObj.TABLE_PAGESIZE);
				$("#dynamictable_Sql").text(tableObj.TABLE_SQL);
				$("#table_columns").empty();
				for (var i = 0; i < tableObj.TABLE_COLIMNS.length; i++) {
					$("#table_columns").append("<option value='" + tableObj.TABLE_COLIMNS[i].value + "'>" + tableObj.TABLE_COLIMNS[i].title + "</option>");
				}


				$("#table_select1_value_record").text(tableObj.FILTER_SELECT1.SELECT_VALUE_RECORD);
				$("#table_select1_text_record").text(tableObj.FILTER_SELECT1.SELECT_TEXT_RECORD);

				if (tableObj.FILTER_SELECT1.SELECT_ENABLE == true)
					$("#table_select1_enable").attr("checked", true);
				else
					$("#table_select1_enable").attr("checked", false);


				$("#table_select1_record").val(tableObj.FILTER_SELECT2.SELECT_TABLE_RECORD);
				$("#table_select1_sql").text(tableObj.FILTER_SELECT2.SELECT_SQL);


				$("#table_select2_value_record").text(tableObj.FILTER_SELECT2.SELECT_VALUE_RECORD);
				$("#table_select2_text_record").text(tableObj.FILTER_SELECT2.SELECT_TEXT_RECORD);

				if (tableObj.FILTER_SELECT1.SELECT_ENABLE == true)
					$("#table_select2_enable").attr("checked", true);
				else
					$("#table_select2_enable").attr("checked", false);


				$("#table_select2_record").val(tableObj.FILTER_SELECT2.SELECT_TABLE_RECORD);
				$("#table_select2_sql").text(tableObj.FILTER_SELECT2.SELECT_SQL);


 

				if (tableObj.FILTER_KEY.TEXT_ENABLE == true)
					$("#table_keysearch_enable").attr("checked", true);
				else
					$("#table_keysearch_enable").attr("checked", false);

				$("#table_keysearch_record").val(tableObj.FILTER_KEY.TABLE_RECORD);

				if (tableObj.FILTER_DATE_RANGE.TEXT_ENABLE == true)
					$("#table_dataesearch_enable").attr("checked", true);
				else
					$("#table_dataesearch_enable").attr("checked", false);
				$("#table_dataesearch_record").val(tableObj.FILTER_DATE_RANGE.TABLE_RECORD );



				if (tableObj.FILTER_SELECT1_CHANGE_SELECT2 == true)
					$("#dynamictable_select_change_enable").attr("checked", true);
				else
					$("#dynamictable_select_change_enable").attr("checked", false);
				

			




			}

		});

		//dynamicTable_propertiessave
		$("#dynamicTable_propertiessave").click(function (e) {
			//???????????????????????????
			e.preventDefault();
			$("#dynamicTable_info").html("");
			if ( $("#dynamictable_Height").val() == "") {
				tableObj.TABLE_HEIGHT = 300;
			}
			if ($("#dynamictable_DataSource").val() == "") {
				$("#dynamicTable_info").html("??????????????????");
				return
			}
			if ($("#dynamictable_Title").val() == "") {
				$("#dynamicTable_info").html("?????????????????????");
				return
			}
			if ($("#dynamictable_KeyId").val() == "") {
				$("#dynamicTable_info").html("????????????????????????ID??????");
				return
			}
			if ($("#dynamictable_pagesize").val() == "") {
				tableObj.TABLE_PAGESIZE = 100;
			}
			if ($("#table_columns option").length<=0) {
				$("#dynamicTable_info").html("????????????????????????");
				return
			}
			if ($("#dynamictable_Sql").val == "") {
				$("#dynamicTable_info").html("????????????????????????sql??????");
				return
			}
			if ($("#table_select1_enable").is(":checked")) {
				if ($("#table_select1_sql").val == "") {
					$("#dynamicTable_info").html("??????????????????????????????sql??????");
					return
				}
				if ($("#table_select1_value_record").val == "") {
					$("#dynamicTable_info").html("?????????????????????????????????");
					return
				}
				if ($("#table_select1_text_record").val == "") {
					$("#dynamicTable_info").html("??????????????????????????????????????????");
					return
				}
				if ($("#table_select1_record").val == "") {
					$("#dynamicTable_info").html("??????????????????????????????????????????????????????");
					return
				}
			}
			if ($("#table_select2_enable").is(":checked")) {
				if ($("#table_select2_sql").val == "") {
					$("#dynamicTable_info").html("??????????????????????????????sql??????");
					return
				}

		 

				if ($("#table_select2_value_record").val == "") {
					$("#dynamicTable_info").html("?????????????????????????????????");
					return
				}
				if ($("#table_select2_text_record").val == "") {
					$("#dynamicTable_info").html("??????????????????????????????????????????");
					return
				}
				if ($("#table_select2_record").val == "") {
					$("#dynamicTable_info").html("??????????????????????????????????????????????????????");
					return
				}
			}

			if ($("#table_keysearch_enable").is(":checked")) {
				if ($("#table_keysearch_record").val == "") {
					$("#dynamicTable_info").html("???????????????????????????????????????");
					return
				}
			 
			}
			if ($("#table_dataesearch_enable").is(":checked")) {
				if ($("#table_dataesearch_record").val == "") {
					$("#dynamicTable_info").html("????????????????????????????????????");
					return
				}

			}
			//????????????????????????????????????
			var tabletitle = protertieseditor.parent().parent().parent().parent().find('div[data-filter="tabletitle"]');

			var tablefilter = protertieseditor.parent().parent().parent().parent().find('div[data-filter="searchbar"]');
			var jsJson = protertieseditor.parent().parent().parent().parent().parent().parent().find('script[tablejson="true"]');//??????json????????????
			var tableObj = {};
			tableObj.DATASOURCE = $("#dynamictable_DataSource").val();
			tableObj.ELE_ID = $("#dynamictable_ID").val();
		
			tableObj.TABLE_TITLE = $("#dynamictable_Title").val();
			tableObj.TABLE_HEIGHT = $("#dynamictable_Height").val();
			tableObj.TABLE_KEY_ID_RECORD = $("#dynamictable_KeyId").val();

			if ($("#dynamictable_toolenable").is(":checked"))
				tableObj.TABLE_SHOW_TOOL = true;
			else
				tableObj.TABLE_SHOW_TOOL = false;

		 
		 

			if ($("#dynamictable_pagenable").is(":checked"))
				tableObj.TABLE_SHOW_PAGE = true;
			else
				tableObj.TABLE_SHOW_PAGE = false;


			if ($("#dynamictable_select_change_enable").is(":checked"))
				tableObj.FILTER_SELECT1_CHANGE_SELECT2 = true;
			else
				tableObj.FILTER_SELECT1_CHANGE_SELECT2 = false;


			tableObj.TABLE_PAGESIZE = $("#dynamictable_pagesize").val();
			tableObj.TABLE_SQL = $("#dynamictable_Sql").val();
			tableObj.TABLE_COLIMNS = [];
			$("#table_columns option").each(function () {

				tableObj.TABLE_COLIMNS.push({ value: $(this).val(), title: $(this).text() });
			});

			tableObj.FILTER_SELECT1 = {};
			tableObj.FILTER_SELECT1.SELECT_VALUE_RECORD = $("#table_select1_value_record").val();
			tableObj.FILTER_SELECT1.SELECT_TEXT_RECORD = $("#table_select1_text_record").val();

			if ($("#table_select1_enable").is(":checked"))
				tableObj.FILTER_SELECT1.SELECT_ENABLE= true;
			else
				tableObj.FILTER_SELECT1.SELECT_ENABLE = false;
			tableObj.FILTER_SELECT1.SELECT_TABLE_RECORD=$("#table_select1_record").val();
			tableObj.FILTER_SELECT1.SELECT_SQL = $("#table_select1_sql").val();
		 

			tableObj.FILTER_SELECT2 = {};
			tableObj.FILTER_SELECT2.SELECT_VALUE_RECORD = $("#table_select2_value_record").val();
			tableObj.FILTER_SELECT2.SELECT_TEXT_RECORD = $("#table_select2_text_record").val();

			if ($("#table_select1_enable").is(":checked"))
				tableObj.FILTER_SELECT2.SELECT_ENABLE = true;
			else
				tableObj.FILTER_SELECT2.SELECT_ENABLE = false;
			tableObj.FILTER_SELECT2.SELECT_TABLE_RECORD=	$("#table_select2_record").val();
			tableObj.FILTER_SELECT2.SELECT_SQL = $("#table_select2_sql").val();


			tableObj.FILTER_KEY = {};
			if ($("#table_keysearch_enable").is(":checked"))
				tableObj.FILTER_KEY.TEXT_ENABLE = true;
			else
				tableObj.FILTER_KEY.TEXT_ENABLE = false;
			tableObj.FILTER_KEY.TABLE_RECORD = $("#table_keysearch_record").val();

			tableObj.FILTER_DATE_RANGE = {};

			if ($("#table_dataesearch_enable").is(":checked"))
				tableObj.FILTER_DATE_RANGE.TEXT_ENABLE = true;
			else
				tableObj.FILTER_DATE_RANGE.TEXT_ENABLE = false;
			tableObj.FILTER_DATE_RANGE.TABLE_RECORD = $("#table_dataesearch_record").val();

			var cnum = tableObj.ELE_ID.split('-')[1];
			var select1Id = "select1-" + cnum;
			var select2Id = "select2-" + cnum;
			var textkeyId = "text-" + cnum;
			var daterange1Id = "date1-" + cnum;
			var daterange2Id = "date2-" + cnum;
			var buttonId = "button-" + cnum;
			var jsonId = "json-" + cnum;
			tableObj.BUTTON_ID = buttonId;
			tableObj.FILTER_SELECT1.ELE_ID = select1Id;
			tableObj.FILTER_SELECT2.ELE_ID = select2Id;
			tableObj.FILTER_KEY.ELE_ID = textkeyId;
			tableObj.FILTER_DATE_RANGE.ELE_ID = daterange1Id;
			tableObj.FILTER_DATE_RANGE.ELE2_ID = daterange2Id;
			var select1Ctrl = tablefilter.find('div[data-filter="select1"]>select');
			if (select1Ctrl != undefined && select1Ctrl.length > 0) {
				select1Ctrl.attr("id", tableObj.FILTER_SELECT1.ELE_ID);
				select1Ctrl.attr("data-json", jsonId);
			}
			var select2Ctrl = tablefilter.find('div[data-filter="select2"]>select');
			if (select2Ctrl != undefined && select2Ctrl.length > 0) {
				select2Ctrl.attr("id", tableObj.FILTER_SELECT2.ELE_ID);
				select2Ctrl.attr("data-json", jsonId);
			}
			var searchKey = tablefilter.find('div[data-filter="key"]>input[type="text"]');
			if (searchKey != undefined && searchKey.length > 0) {
				searchKey.attr("id", tableObj.FILTER_KEY.ELE_ID);
			
			}

			var date1Ctrl = tablefilter.find('input[data-filter="date1"]');
			if (date1Ctrl != undefined && date1Ctrl.length > 0) {
				date1Ctrl.attr("id", tableObj.FILTER_DATE_RANGE.ELE_ID);
			}
			var date2Ctrl = tablefilter.find('input[data-filter="date2"]');
			if (date2Ctrl != undefined && date2Ctrl.length > 0) {
				date2Ctrl.attr("id", tableObj.FILTER_DATE_RANGE.ELE2_ID);
			}

			var searchButton = tablefilter.find('div[data-filter="searchbutton"]>button[type="button"]');
			if (searchButton != undefined && searchButton.length > 0) {
				searchButton.attr("id", tableObj.BUTTON_ID);
				$(searchButton).click(function (e) {
					$("#" + tableObj.ELE_ID).bootstrapTable('refresh', { url: "/API/API/TableQuery" });

				});
			}
			tableObj.FILTER_SELECT1.DATASOURCE = tableObj.DATASOURCE;
			tableObj.FILTER_SELECT2.DATASOURCE = tableObj.DATASOURCE;
			
			jsJson.html(JSON.stringify(tableObj));
			jsJson.attr("id", jsonId);
	        //??????????????????????????????????????????
			tabletitle.html(tableObj.TABLE_TITLE);
			if (tableObj.TABLE_SHOW_TOOL) {
				tablefilter.css("display", "block");
			}
			else {
				tablefilter.css("display", "none");
			}
			if (tableObj.FILTER_SELECT1.SELECT_ENABLE) {
				var select1 = tablefilter.find('div[data-filter="select1"]');
				select1.css("display", "block");

			}
			else {
				var select1 = tablefilter.find('div[data-filter="select1"]');
				select1.css("display", "none");
			}


			if (tableObj.FILTER_SELECT2.SELECT_ENABLE) {
				var select2 = tablefilter.find('div[data-filter="select2"]');
				select2.css("display", "block");
			}
			else {
				var select2 = tablefilter.find('div[ data-filter="select2"]');
				select2.css("display", "none");
			}

			if (tableObj.FILTER_KEY.TEXT_ENABLE) {
				var searchkey = tablefilter.find('div[data-filter="key"]');
				searchkey.css("display", "block");
			}
			else {
				var searchkey = tablefilter.find('div[data-filter="key"]');
				searchkey.css("display", "none");
			}

			if (tableObj.FILTER_DATE_RANGE.TEXT_ENABLE) {
				var daterange = tablefilter.find('div[data-filter="daterange"]');
				daterange.css("display", "block");
			 
			}
			else {
				var daterange = tablefilter.find('div[data-filter="daterange"]');
				daterange.css("display", "none");
		 
			 
			}


			var tablecolumn = [];
			for (var i = 0; i < tableObj.TABLE_COLIMNS.length; i++) {
				tablecolumn.push({
					field: tableObj.TABLE_COLIMNS[i].value,
					title: tableObj.TABLE_COLIMNS[i].title,
					align: 'center',
					valign: 'middle'
				});
			}
		

			$("#" + tableObj.ELE_ID).bootstrapTable('destroy');
	       //??????????????????
			$("#" + tableObj.ELE_ID).bootstrapTable({
				url: "/API/API/TableQuery",         //???????????????URL???*???
				method: 'get',                      //???????????????*???
				//toolbar: '#toolbar',              //???????????????????????????
				striped: true,                      //????????????????????????
				cache: false,                       //??????????????????????????????true?????????????????????????????????????????????????????????*???
				pagination: true,                   //?????????????????????*???
				sortable: true,                     //??????????????????
				sortOrder: "asc",                   //????????????
				sidePagination: "server",           //???????????????client??????????????????server??????????????????*???
				pageNumber: 1,                      //??????????????????????????????????????????,?????????
				pageSize: tableObj.TABLE_PAGESIZE,                     //????????????????????????*???
				pageList: [10, 25, 50, 100, 200, 300, 400, 500, 600, 700, 800, 1000],        //?????????????????????????????????*???
				search: false,                      //????????????????????????
				strictSearch: true,
				showColumns: true,                  //????????????????????????????????????????????????
				showRefresh: true,                  //????????????????????????
				minimumCountColumns: 2,             //?????????????????????
				clickToSelect: true,                //???????????????????????????
				height: tableObj.TABLE_HEIGHT,                      //???????????????????????????height?????????????????????????????????????????????????????????
				uniqueId: tableObj.TABLE_KEY_ID_RECORD,                     //?????????????????????????????????????????????
				showToggle: true,                   //??????????????????????????????????????????????????????
				cardView: false,                    //????????????????????????
				detailView: false,
				 //?????????????????????
				//?????????????????????
				queryParams: function (params) {
					//????????????????????????????????????????????????????????????????????????????????????????????????????????????
					var filter = {
						rows: params.limit,                         //????????????
						page: (params.offset / params.limit) + 1,   //??????
						offset: params.offset,
						select1: $("#" + tableObj.FILTER_SELECT1.ELE_ID).val(),
						select2: $("#" + tableObj.FILTER_SELECT2.ELE_ID).val(),
						startdate: $("#" + tableObj.FILTER_DATE_RANGE.ELE_ID).val(),
						enddate: $("#" + tableObj.FILTER_DATE_RANGE.ELE2_ID).val(),
					};
					tableObj.FILTER = filter;
					return tableObj;
				},

				columns: [tablecolumn],
				responseHandler: function (res) {
					return { "rows": res.data, "total": res.rowsCount };
				},
				onLoadSuccess: function () {
					showTips("?????????????????????");
				},
				onLoadError: function () {
					showTips("?????????????????????");
				},
				onDblClickRow: function (row, $element) {
					var id = row.ID;

				},
			});
			//?????????????????????
			if (tableObj.FILTER_SELECT1.SELECT_ENABLE) {


				$.get("/API/API/SelectQuery", tableObj.FILTER_SELECT1, function (result) {
					var selectEle = $("#" + tableObj.FILTER_SELECT1.ELE_ID);
					selectEle.empty();
					selectEle.append("<option value=''>?????????</option>");
					if (result.state == "success") {

						for (var i = 0; i < result.data.length; i++) {
							selectEle.append("<option value='" + result.data[i].value + "'>" + result.data[i].title + "</option>");
						}

					}


				});
			}
		
			if (tableObj.FILTER_SELECT1_CHANGE_SELECT2) {
				if (tableObj.FILTER_SELECT2.SELECT_ENABLE) {

				 
					$("#" + tableObj.FILTER_SELECT1.ELE_ID).change(function (e) {

						var selectEle = $("#" + tableObj.FILTER_SELECT2.ELE_ID);
						selectEle.empty();
						selectEle.append("<option value=''>?????????</option>");

						$.get("/API/API/SelectQuery", tableObj.FILTER_SELECT2, function (result) {

							if (result.state == "success") {

								for (var i = 0; i < result.data.length; i++) {
									selectEle.append("<option value='" + result.data[i].value + "'>" + result.data[i].title + "</option>");
								}

							}
						});

					});
				}
			}
			else {
				if (tableObj.FILTER_SELECT2.SELECT_ENABLE) {
					$("#" + tableObj.FILTER_SELECT1.ELE_ID).unbind('change');//??????RecommandProduct ??????
					$.get("/API/API/SelectQuery", tableObj.FILTER_SELECT2, function (result) {
						var selectEle = $("#" + tableObj.FILTER_SELECT2.ELE_ID);
						selectEle.empty();
						selectEle.append("<option value=''>?????????</option>");
						if (result.state == "success") {

							for (var i = 0; i < result.data.length; i++) {
								selectEle.append("<option value='" + result.data[i].value + "'>" + result.data[i].title + "</option>");
							}

						}
					});
				}
			}
			$("#DynamicTableModal").modal('hide');
		
		});
		//?????????????????????
		$("#table_column_add").click(function (e) {
			e.preventDefault();
			if ($("#table_column_title").val() == "") {
				WebDesigner.showMessage("??????????????????");
				return;
            }
			if ($("#table_column_record").val() == "") {
				WebDesigner.showMessage("??????????????????????????????????????????");
				return;
			}
			$("#table_columns").append("<option value =\"" + $("#table_column_record").val() + "\" > " + $("#table_column_title").val() + "</option>");
			$("#table_column_record").val("");
			$("#table_column_title").val("");
			return false;
		});
		//?????????????????????
		$("#table_column_del").click(function (e) {
			e.preventDefault();
			$("#table_columns").find("option[value=" + $("#table_columns").val() + "]").remove();
		 

		});
		$("#table_column_empty").click(function (e) {
			e.preventDefault();
			$("#table_columns").empty();
		});
		//????????????
		$('body.edit .demo').on("click", "[data-target='#dynamicselectModal']", function (e) {
			//??????????????????
			e.preventDefault();
			protertieseditor = $(this).parent().parent().parent().find('select[propertyeditable="true"]');
			var jsJson = $(this).parent().parent().parent().find('script[selectjson="true"]');//??????json????????????

			var tableObj = JSON.parse(jsJson.html());
			if (tableObj != undefined && tableObj != null) {

				tableObj.ELE_ID = protertieseditor[0].id;
				var cnum = tableObj.ELE_ID.split('-')[1];
				 

				$("#dynamicselect_DataSource").val(tableObj.DATASOURCE);//???????????????
				$("#dynamicselect_ID").val(tableObj.ELE_ID);
				$("#dynamicselect_Sql").val(tableObj.SELECT_SQL);
				$("#dynamicselect_Value").val(tableObj.SELECT_VALUE_RECORD);
				$("#dynamicselect_Text").val(tableObj.SELECT_TEXT_RECORD);
				 
			}

		});
		$("#dynamicselect_propertiessave").click(function (e) {
			if ($("#dynamicselect_DataSource").val() == "") {
				$("#dynamicSelect_info").html("??????????????????");
				return
			}
			if ($("#dynamicselect_Value").val() == "") {
				$("#dynamicSelect_info").html("??????????????????");
				return
			}
			if ($("#dynamicselect_Text").val() == "") {
				$("#dynamicSelect_info").html("???????????????????????????");
				return
			}
			if ($("#dynamicselect_Sql").val() == "") {
				$("#dynamicSelect_info").html("??????????????????sql??????");
				return
			}

			var jsJson = protertieseditor.parent().parent().parent().parent().parent().parent().find('script[selectjson="true"]');//??????json????????????
			var tableObj = {};
			tableObj.DATASOURCE = $("#dynamicselect_DataSource").val();
			tableObj.ELE_ID = $("#dynamicselect_ID").val();
			tableObj.SELECT_SQL = $("#dynamicselect_Sql").val();
			tableObj.SELECT_VALUE_RECORD = $("#dynamicselect_Value").val();
			tableObj.SELECT_TEXT_RECORD = $("#dynamicselect_Text").val();

			jsJson.html(JSON.stringify(tableObj));
	 
			$.get("/API/API/SelectQuery", tableObj, function (result) {
				var selectEle = $("#" + tableObj.ELE_ID);
				selectEle.empty();
				if (result.state == "success") {

					for (var i = 0; i < result.data.length; i++) {
						selectEle.append("<option value='" + result.data[i].value + "'>" + result.data[i].title + "</option>");
					}

				}
			});
			$("#dynamicselectModal").modal('hide');

		});
		$("#btPageNew").click(function (e) {
			e.preventDefault();
			if ($("#tbPageTitle").val() == "") {
				WebDesigner.showMessage("?????????????????????");
			}
			else {
				$.post("/Scada/ScaddDesigner/AddPage", { "title": $("#tbPageTitle").val(), "remark": $("#tbPageRemark").val() }, function (result) {
					if (result.code == 0) {
						$("#newPage").modal('hide');
						$("#PageId").val(result.data.Id);
						$("#pageTitle").html("???????????????????????????" + result.data.PageTitle);
						$("#jsFile").val(result.data.JsContent);
						$("#PageUid").val(result.data.PageUid);

						restoreFromDbData("");

						WebDesigner.LoadPages();
					}
					else {
						WebDesigner.showMessage("??????????????????");
					}

				});
			}
		});


		$("#save").click(function (e) {
			e.preventDefault();
			if ($("#PageId").val() != "" && $("#PageId").val() != 0) {
				saveLayoutToDb();
			}
		});
		//?????????????????????
		$("#testDatasource").click(function (e) {
			e.preventDefault();
			if ($("#dynamictable_DataSource").val() != "" && $("#dynamictable_DataSource").val() != 0) {
				$.post("/Scada/ScaddDesigner/DataSourceTest", { "Id": $("#dynamictable_DataSource").val() }, function (result) {
					WebDesigner.showMessage(result.message);
					 
				});

			}
		});

	}
 
	this.showMessage = function (msg) {
		$("#mesgContain").html(msg);
		$("#MessageBox").modal('show');

	}
	//??????????????????????????????????????????
	this.InitWebPage = function ()
	{
		var tablejson = $('script[tablejson="true" ]');//???????????????tables???????????????json
		for (var i = 0; i < tablejson.length; i++) {
			var tableObj = JSON.parse(tablejson[i].html());
			var tablecolumn = [];
			for (var i = 0; i < tableObj.TABLE_COLIMNS.length; i++) {
				tablecolumn.push({
					field: tableObj.TABLE_COLIMNS[i].value,
					title: tableObj.TABLE_COLIMNS[i].title,
					align: 'center',
					valign: 'middle'
				});
			}
			$("#" + tableObj.ELE_ID).bootstrapTable('destroy');
			//??????????????????
			$("#" + tableObj.ELE_ID).bootstrapTable({
				url: "/API/API/TableQuery",         //???????????????URL???*???
				method: 'get',                      //???????????????*???
				//toolbar: '#toolbar',              //???????????????????????????
				striped: true,                      //????????????????????????
				cache: false,                       //??????????????????????????????true?????????????????????????????????????????????????????????*???
				pagination: tableObj.TABLE_SHOW_PAGE,                   //?????????????????????*???
				sortable: true,                     //??????????????????
				sortOrder: "asc",                   //????????????
				sidePagination: "server",           //???????????????client??????????????????server??????????????????*???
				pageNumber: 1,                      //??????????????????????????????????????????,?????????
				pageSize: tableObj.TABLE_PAGESIZE,                     //????????????????????????*???
				pageList: [10, 25, 50, 100, 200, 300, 400, 500, 600, 700, 800, 1000],        //?????????????????????????????????*???
				search: false,                      //????????????????????????
				strictSearch: true,
				showColumns: true,                  //????????????????????????????????????????????????
				showRefresh: true,                  //????????????????????????
				minimumCountColumns: 2,             //?????????????????????
				clickToSelect: true,                //???????????????????????????
				height: tableObj.TABLE_HEIGHT,                      //???????????????????????????height?????????????????????????????????????????????????????????
				uniqueId: tableObj.TABLE_KEY_ID_RECORD,                     //?????????????????????????????????????????????
				showToggle: true,                   //??????????????????????????????????????????????????????
				cardView: false,                    //????????????????????????
				detailView: false,
				//?????????????????????
				//?????????????????????
				queryParams: function (params) {
					//????????????????????????????????????????????????????????????????????????????????????????????????????????????
					var filter = {
						rows: params.limit,                         //????????????
						page: (params.offset / params.limit) + 1,   //??????
						offset: params.offset,
						allowpage: tableObj.TABLE_SHOW_PAGE,
						select1: $("#" + tableObj.FILTER_SELECT1.ELE_ID).val(),
						select2: $("#" + tableObj.FILTER_SELECT2.ELE_ID).val(),
						startdate: $("#" + tableObj.FILTER_DATE_RANGE.ELE_ID).val(),
						enddate: $("#" + tableObj.FILTER_DATE_RANGE.ELE2_ID).val(),
						key: $("#" + tableObj.FILTER_KEY.ELE_ID).val(),
					};
					tableObj.FILTER = filter;
					return tableObj;
				},

				columns: [tablecolumn],
				responseHandler: function (res) {
					return { "rows": res.data, "total": res.rowsCount };
				},
				onLoadSuccess: function () {
					showTips("?????????????????????");
				},
				onLoadError: function () {
					showTips("?????????????????????");
				},
				onDblClickRow: function (row, $element) {
					var id = row.ID;

				},
			});
			$("#" + tableObj.BUTTON_ID).click(function (e) {
				$("#" + tableObj.ELE_ID).bootstrapTable('refresh', { url: "/API/API/TableQuery" });
			});
			//???????????????????????????
			if (tableObj.TABLE_SHOW_TOOL) {
				$("#" + tableObj.FILTER_SELECT1.ELE_ID).unbind('change');//??????RecommandProduct ??????
				if (tableObj.FILTER_SELECT1.SELECT_ENABLE) {


					$.get("/API/API/SelectQuery", tableObj.FILTER_SELECT1, function (result) {
						var selectEle = $("#" + tableObj.FILTER_SELECT1.ELE_ID);
						selectEle.empty();
						selectEle.append("<option value=''>?????????</option>");
						if (result.state == "success") {

							for (var i = 0; i < result.data.length; i++) {
								selectEle.append("<option value='" + result.data[i].value + "'>" + result.data[i].title + "</option>");
							}

						}
					});
				}

				if (tableObj.FILTER_SELECT2.SELECT_ENABLE && tableObj.FILTER_SELECT1_CHANGE_SELECT2 == false) {


					$.get("/API/API/SelectQuery", tableObj.FILTER_SELECT2, function (result) {
						var selectEle = $("#" + tableObj.FILTER_SELECT2.ELE_ID);
						selectEle.empty();
						selectEle.append("<option value=''>?????????</option>");
						if (result.state == "success") {

							for (var i = 0; i < result.data.length; i++) {
								selectEle.append("<option value='" + result.data[i].value + "'>" + result.data[i].title + "</option>");
							}

						}
					});
				}

				if (tableObj.FILTER_SELECT1_CHANGE_SELECT2) {

				
					$("#" + tableObj.FILTER_SELECT1.ELE_ID).change(function (e) {

						var selectEle = $("#" + tableObj.FILTER_SELECT2.ELE_ID);
						selectEle.empty();
						selectEle.append("<option value=''>?????????</option>");

						$.get("/API/API/SelectQuery", tableObj.FILTER_SELECT2, function (result) {

							if (result.state == "success") {

								for (var i = 0; i < result.data.length; i++) {
									selectEle.append("<option value='" + result.data[i].value + "'>" + result.data[i].title + "</option>");
								}

							}
						});

					});

                }
				 

            }
		}
		//????????????????????????
		var chartjson = $('script[charjson="true" ]');//???????????????chart???????????????json

		//????????????????????????????????????
		var selectjson = $('script[selectjson="true" ]');//???????????????select???????????????json
		for (var i = 0; i < selectjson.length; i++) {
			var tableObj = JSON.parse(selectjson[i].html());
			$("#" + tableObj.ELE_ID).empty();
			$.get("/API/API/SelectQuery", tableObj, function (result) {
				var selectEle = $("#" + tableObj.ELE_ID);
				selectEle.empty();
				if (result.state == "success") {

					for (var i = 0; i < result.data.length; i++) {
						selectEle.append("<option value='" + result.data[i].value + "'>" + result.data[i].title + "</option>");
					}

				}
			});
		}
    }
}
var WebDesigner = new SCADA_WebDesigner();