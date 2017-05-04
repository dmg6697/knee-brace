var flexion = 0;
var orientation = Math.PI;
var force = 50;
var force_text = 0;
var force_max = 100;
var force_min = 0;
var graph_width = 20;
var force_history = [];

$(document).ready(function () {
    ShowLoading();

	$(window).resize(function()
	{
		Redraw();
	});

	$('.input-group label').hide();
	$('input, select').css({ width: '100%', 'margin-bottom': '10px' });
	$('button').css({ 'margin-bottom': '10px' });
	$('.input-group input').on('input', function () {
	    var $this = $(this);
	    if ($this.val() != "") {
	        $this.parent().find('label').show({ duration: 200 });
	    } else {
	        $this.parent().find('label').hide();
	    }
	});

	$('#settings_btn').click(ShowSettings);
	$("#content_btn").click(ShowContent);

	$('select').on('change', function () {
	    $('#connectionStatus').css({ color: "black" }).text('Untested');
	});

	$('#configure_btn').click(function () {
	    InvokeCSharp({ type: 'settings', f: $('#Frequency').val(), k: $('#SpringK').val() });
	});

	$('#tryConnBtn').click(function () {
	    InvokeCSharp({ type: 'connection', port: $('#COMPort').val() });
	    $(this).attr('disabled', 'disabled');
	});
	
	Redraw();
});

function contentVisibleChanged(show) {
    var btn = $('#settings_btn');
    var content = $('#content');
    var settings = $('#settings');

    if (show) {
        content.hide();
        settings.show();
        btn.text('Display');
    }
    else {
        content.show();
        settings.hide();
        btn.text('Settings');
    }
}

function ShowLoading() {
    $('#content, #settings').hide();
    $("#loading").show();
}

function ShowContent() {
    $('#loading, #settings').hide();
    $("#content").show();
}

function ShowSettings() {
    $('#loading, #content').hide();
    $('#settings').show();
}

function Redraw()
{
	var $window = $(window);
	var h = $window.height() * 0.9;
	var w = $window.width() * 0.9;
	var canvas = document.getElementById('kp_canvas');
	canvas.height = h;
	canvas.width = w;
	var ctx = canvas.getContext('2d', w, h);

    // draw border and knee position
	var w_knee = w - graph_width - 3;
	DrawBorder(ctx, w_knee, h/2);
	DrawKnee(ctx, w_knee, h / 2, flexion, orientation);

    // draw force graph (instant)
	ctx.translate(w_knee, 10); // top left = new coords
	ctx.moveTo(0, 0);
	DrawIForce(ctx, graph_width - 3, h, force);
	ctx.translate(-w, -10); // restore old coords
	ctx.moveTo(0, 0);

    // draw force time graph
	force_history.push(force);
	if (force_history.length > 20)
	{
	    force_history.shift();
	}
	DrawTimeForce(ctx, w_knee, h / 2, 0, h - 10);

    // draw force text
	DrawForceText(ctx, force_text, w_knee / 2, h / 2 + 30);
}

function DrawTimeForce(ctx, w, h, x, y)
{
    // relative move
    ctx.translate(x, y);
    var w_unit = w / force_history.length; // get unit length
    var h_unit = h / 100;

    // assemble points & connect them
    ctx.beginPath();

    for (var i = 0; i < force_history.length; i++)
    {
        ctx.lineTo(w_unit * i, h_unit * -1 * force_history[i]);
    }
    ctx.stroke();

    // move back
    ctx.translate(-x, -y);
}

function roundTo(num, places)
{
    return +(Math.round(num + "e+" + places) + "e-" + places);
}

function DrawBorder(ctx, width, height)
{
	var x = width / 2;
	var y = height / 2;
	var r = Math.min(x, y) * 0.1;
	var r2 = Math.min(x, y) * 0.8;
	ctx.beginPath();
	ctx.arc(x, y, r2, 0, 2*Math.PI, true);
	ctx.stroke();
	ctx.beginPath();
	ctx.arc(x, y, r, 0, 2*Math.PI, true);
	ctx.stroke();
}

function DrawIForce(ctx, width, height, force)
{
    // draw outline
    ctx.beginPath();
    ctx.rect(0, 0, width, height);
    ctx.stroke();

    // draw force
    ctx.beginPath();
    var delta = height - (height * (force / (force_max - force_min)));
    ctx.rect(0, delta, width, height);
    var gradient = ctx.createLinearGradient(0, 0, 0, height);
    gradient.addColorStop(0, "blue");
    gradient.addColorStop(1, "white");
    ctx.fillStyle = gradient;
    ctx.fill();
}

function DrawKnee(ctx, width, height, flexion, orientation)
{
	var x = width / 2;
	var y = height / 2;
	var r = Math.min(x, y);
	
	ctx.translate(x, y); // new center
	ctx.beginPath();
	var lxe = r * Math.sin(flexion);
	var lye = r * Math.cos(flexion);
	
	var uxe = r * Math.sin(orientation);
	var uye = r * Math.cos(orientation);
	
	ctx.moveTo(0, 0);
	ctx.lineTo(lxe, lye);
	ctx.stroke();
	ctx.beginPath();
	
	ctx.beginPath();
	ctx.moveTo(0, 0);
	ctx.lineTo(uxe, uye);
	ctx.stroke();
	
	ctx.translate(-x, -y); // return to top left
}

function DrawForceText(ctx, forceLbs, x, y)
{
    ctx.beginPath();
    ctx.font = "15px Georgia";
    ctx.fillText("Force (ft-lbs): " + roundTo(forceLbs, 2), x, y);
    ctx.stroke();
    ctx.beginPath();
}

function Update(iforce, iflexion, iorientation, pforce)
{
    force = pforce;
    flexion = iflexion;
    orientation = iorientation;
    force_text = iforce;

    Redraw();
}

function InvokeCSharp(event)
{
    csharp.raiseEvent(event);
}

function connectionChanged(isConnected, portNames)
{
    var ports = portNames.split(',');

    var select = $('#COMPort');
    var option = "";
    for (var i = 0; i < ports.length; i++) {
        option += "<option value='" + ports[i] + "'>" + ports[i] + "</option>";
    }
    select.html(option);

    var trybtn = $('#tryConnBtn');
    if (trybtn.attr('disabled') == 'disabled')
    {
        trybtn.removeAttr('disabled');
    }
    
    $('#connectionStatus').css({
        'color': isConnected ? 'green' : 'red',
    }).text(isConnected ? 'Connected' : 'Not connected');

    if (!isConnected)
    {
        ShowSettings();
    } else {
        ShowContent();
    }
}

function settingsUpdated(updated)
{
    if (!updated) {
        alert('bad update');
    }
}