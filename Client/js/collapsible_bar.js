$('.btn-expand-collapse').click(function(e) {
	$('.navbar-primary').toggleClass('collapsed');
	if($('.navbar-primary').hasClass('collapsed')) 
		$('#toggleicon').removeClass("glyphicon-menu-left").addClass("glyphicon-menu-right");
	else 
		$('#toggleicon').removeClass("glyphicon-menu-right").addClass("glyphicon-menu-left");
});