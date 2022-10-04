        function SetContentEdit() {
	CKEDITOR.disableAutoInline = true;

	var contenthandle = CKEDITOR.replace('contenteditor', {
		language: 'zh-cn',
		contentsCss: ['/Content/css/bootstrap-combined.min.css'],
		allowedContent: true
	});
}
function restoreFromDbData(html) {
	if (html == null || html == "")
		html = "";
 
	restoreDataFromHtml(html);
	saveLayout();
}