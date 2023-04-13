//Window Gestion Ventana Modal
function winGestion() {
	App.winGestion.show();
}

function anadir() {
	winGestion();

}

function editar() {
	winGestion();
}


function habilitaLnk(vago) {
	let ct = document.getElementById('cntNavVistasForm-innerCt');
	let aLinks = ct.querySelectorAll('a');

	aLinks.forEach(function(itm) {
	itm.classList.remove("navActivo");
	});

	vago.classList.add('navActivo');
}

function showFlow(FlOk, FlobjOk) {
	let ct = App.pnContent.items.items;
	let flows = document.querySelectorAll('.pnDiagramFlow');

	for (i = 0; i < 3; i++) {//objetos
		ct[i].hidden = true;
	}

	flows.forEach(function (fl) {
		fl.classList.add('d-none');
	});

	FlOk.classList.remove('d-none');
	FlobjOk.show();
}


function hideForms(dvVisible, objVisible) {
	let ctF = App.pnContent.items.items;
	let frms = document.querySelectorAll('.formWorkflow');

	for (i = 0; i < ctF.length; i++) {//objetos
		ctF[i].hidden = true;

	}

	frms.forEach(function (itm) {//divs
		itm.classList.add('d-none');
	});

	dvVisible.classList.remove('d-none');
	objVisible.show();
	
}


function showForms(who) {
	var LNo = who.textEl;
	let Fok;
	let FobjOk;
	let FlOk;
	let FlobjOk;

	switch (who.id) {
		case 'lnkState':
			Fok = document.getElementById('formState');
			FlOk = document.getElementById('pnDiagramFlow0');
			FobjOk = App.formState;
			FlobjOk = App.pnDiagramFlow0;
			habilitaLnk(LNo);
			hideForms(Fok, FobjOk);
			showFlow(FlOk, FlobjOk);
			
		break;

		case 'lnkSubprocess':
			Fok = document.getElementById('formSubP');
			FlOk = document.getElementById('pnDiagramFlow0');
			FobjOk = App.formSubP;
			FlobjOk = App.pnDiagramFlow0;
			habilitaLnk(LNo);
			hideForms(Fok, FobjOk);
			showFlow(FlOk, FlobjOk);
			
		break;

		case 'lnkNext':
			Fok = document.getElementById('formNext');
			FlOk = document.getElementById('pnDiagramFlow');
			FobjOk = App.formNext;
			FlobjOk = App.pnDiagramFlow;
			habilitaLnk(LNo);
			hideForms(Fok, FobjOk);
			showFlow(FlOk, FlobjOk);
			
			break;

		case 'lnkLinks':
			Fok = document.getElementById('formLinks');
			FlOk = document.getElementById('pnDiagramFlowLnk');
			FobjOk = App.formLinks;
			FlobjOk = App.pnDiagramFlowLnk;
			habilitaLnk(LNo);
			hideForms(Fok, FobjOk);
			showFlow(FlOk, FlobjOk);
			
			break;

		case 'lnkNots':
			Fok = document.getElementById('formNots');
			FlOk = document.getElementById('pnDiagramFlow0');
			FobjOk = App.formNots;
			FlobjOk = App.pnDiagramFlow0;
			habilitaLnk(LNo);
			hideForms(Fok, FobjOk);
			showFlow(FlOk, FlobjOk);
			
			break;

		case 'lnkDocs':
			Fok = document.getElementById('formDocs');
			FlOk = document.getElementById('pnDiagramFlow0');
			FobjOk = App.formDocs;
			FlobjOk = App.pnDiagramFlow0;
			habilitaLnk(LNo);
			hideForms(Fok, FobjOk);
			showFlow(FlOk, FlobjOk);
			
			break;

		case 'lnkFunc':
			Fok = document.getElementById('formFuncs');
			FlOk = document.getElementById('pnDiagramFlow0');
			FobjOk = App.formFuncs;
			FlobjOk = App.pnDiagramFlow0;
			habilitaLnk(LNo);
			hideForms(Fok, FobjOk);
			showFlow(FlOk, FlobjOk);
			
			break;
	}
}


//FIN Window Gestion Ventana Modal



// Renders Grid
var barGrid = function (value) {

	let colorBar;

	if (value > 0 && value < 0.20) {
		colorBar = 'barRed-grid';
	}
	else if (value >= 0.20 && value < 0.45) {
		colorBar = 'barYellow-grid';
	}

	else if (value >= 0.45 && value < 0.80) {
		colorBar = 'barBlue-grid';
	}

	else if (value >= 0.80 && value <= 1) {
		colorBar = 'barGreen-grid';
	}
	return `<div class="x-progress x-progress-default" style="margin:2px 1px 1px 1px;width:50px;">
				<div class="x-progress-text x-progress-text-back" style="width:50px;">${value * 100}%</div>
				<div class="x-progress-bar x-progress-bar-default ${colorBar}" style="width: ${value * 100}%;"><div class="x-progress-text" style="width:40px;"><div>${value * 100} %</div></div></div></div>`

}

var rojoRender = function (value) {
	let valorRojo = value;

	if (value != null || value != "") {
		return '<span class="dataRed">' + valorRojo + '</span>';
	}
	else {
		return '<span>&nbsp;</span>';
	}
}

var amarilloRender = function (value) {
	let valorAmarillo = value;

	if (value != null || value != "") {
		return '<span class="dataYellow">' + valorAmarillo + '</span>';
	}
	else {
		return '<span>&nbsp;</span>';
	}
}

var DefectoRender = function (value) {
	if (value == "icon") {
		return '<span class="ico-defaultGrid">&nbsp;</span>'
	}
	else {
		return '<span>&nbsp;</span> '
	}
}

var DocsRender = function (value) {
	if (value == "icon") {
		return '<span class="ico-docsGrid">&nbsp;</span>'
	}
	else {
		return '<span>&nbsp;</span> '
	}
}

var FunctRender = function (value) {
	if (value == "icon") {
		return '<span class="ico-functionalityGrid">&nbsp;</span>'
	}
	else {
		return '<span>&nbsp;</span> '
	}
}

var LinkRender = function (value) {
	if (value == "icon") {
		return '<span class="ico-linkGrid">&nbsp;</span>'
	}
	else {
		return '<span>&nbsp;</span> '
	}
}

var NotifRender = function (value) {
	if (value == "icon") {
		return '<span class="ico-notificationGrid">&nbsp;</span>'
	}
	else {
		return '<span>&nbsp;</span> '
	}
}

var SubprocessRender = function (value) {
	if (value == "icon") {
		return '<span class="ico-subprocessGrid">&nbsp;</span>'
	}
	else {
		return '<span>&nbsp;</span> '
	}
}
// FIN Renders Grid

// Funcionalidades Grid
function ShowWorkFlow() {
	
	if (App.pnShowWorkflow.hidden == true) {
		App.pnShowWorkflow.show();
	}
	else {
		App.pnShowWorkflow.hide();
	}
}

function showPnAttributes() {
	let pnAtt = document.getElementById('pnAsideRight');
		pnAtt.style.width = '400px';
		App.pnAsideRight.show();
	}


function hidePnAttributes() {
	let pnAtt = document.getElementById('pnAsideRight');
	pnAtt.style.width = '0';

}

// FIN Funcionalidades Grid

//Panel atributos


function slider(oBtn) {
	let btnName = oBtn.id;
	let btn = document.getElementById(btnName);
	let sldr = btn.parentNode.parentNode.parentNode;
	let u = sldr.childNodes[3];
	let num = btnName.charAt(btnName.length - 1);
	let allBtn = sldr.querySelectorAll('a');

	
	allBtn.forEach(function (el) {
		el.classList.remove("btnDot-activo");
	});

	switch (num) {

		case '1':
			u.style.marginLeft = '0px';
			allBtn[0].classList.add("btnDot-activo");
			
			break;

		case '2':
			u.style.marginLeft = '-390px';
			allBtn[1].classList.add("btnDot-activo");
			
			break;

		case '3':
			u.style.marginLeft = '-790px';
			//allBtn[2].classList.add("btnDot-activo");
			//console.log(allBtn[2]);
			break;

		default:
			u.style.marginLeft = '0px';
			//allBtn[3].classList.add("btnDot-activo");
			//console.log(allBtn[3]);
			
	}

}


function allRecipients() {
	let txt = App.lblRecipients.text;
	let dv = document.getElementById('dvAllRecipients');
	dv.classList.remove('d-none');
	dv.innerHTML = `<a href="#" id="aCloseMessage" onclick="closeMessage(dvAllRecipients);">Close</a> <h5 id="hEntireMessage" class="labelSlide">All Recipients</h5>${txt}`;
}


function closeMessage(box) {
	box.classList.add('d-none');
	box.innerHTML = ``;
}

function entireMessage() {
	let txt = App.lblMessage.text;
	let dv = document.getElementById('dvEntireMessage');
	dv.classList.remove('d-none');
	dv.innerHTML = `<a href="#" id="aCloseMessage" onclick="closeMessage(dvEntireMessage);">Close</a> <h5 id="hEntireMessage" class="labelSlide">Entire Message</h5>${txt}`;
}


//FIN Panel atributos


