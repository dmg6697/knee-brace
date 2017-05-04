var flexion = 0;
var orientation = Math.PI;
var force = 50;
var force_max = 100;
var force_min = 0;
var graph_width = 20;

$(document).ready(function() {	
	$(window).resize(function()
	{
		Redraw();
	});

	Redraw();
});

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
	DrawBorder(ctx, w_knee, h);
	DrawKnee(ctx, w_knee, h, flexion, orientation);

    // draw force graph (instant)
	ctx.translate(w_knee, 10); // top left = new coords
	ctx.moveTo(0, 0);
	DrawIForce(ctx, graph_width - 3, h, force);
	ctx.translate(-w, -10); // restore old coords
	ctx.moveTo(0, 0);
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

function Update(iforce, iflexion, iorientation, iforce_max, iforce_min)
{
    force = iforce;
    flexion = iflexion;
    orientation = iorientation;
    force_max = iforce_max;
    force_min = iforce_min;

    Redraw();
}