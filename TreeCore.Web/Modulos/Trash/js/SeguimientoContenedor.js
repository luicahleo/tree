//RENDERS ASIDE y MAS

function renderAuthorized(valor, id) {
    let imag = document.getElementById('imAuthorized' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }


}

function renderClosed(valor, id) {
    let imag = document.getElementById('imClsd' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';
    }



}

function renderMultiflow(valor, id) {

    let imag = document.getElementById('imMultiflow' + id);

    if (valor == false) {
        imag.src = '';

    }

    else {
        imag.src = '../../ima/ico-subprocess.svg';
    }


}

function renderCommercial(valor, id) {

    let imag = document.getElementById('imCommercial' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-vendor.svg';

    }


}

function renderInactive(valor, id) {
    let imag = document.getElementById('imInactive' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-cancel.svg';

    }


}

function renderProgBar(valor, id) {
    let bar = document.getElementById('progBar' + id);
    let ancho = valor;

    bar.style.width = ancho * 100 + "%";


}

function renderRegion(valor, id) {
    let imag = document.getElementById('imRegion' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-region-gr.svg';

    }


}

function renderStaff(valor, id) {
    let imag = document.getElementById('imStaff' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }


}

function renderSupport(valor, id) {
    let imag = document.getElementById('imSupport' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }

}

function renderLDAP(valor, id) {
    let imag = document.getElementById('imLDAP' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }

}






var stPn = 0;
function hidePn() {
	let pn = document.getElementById('pnAsideR');
	let btn = document.getElementById('btnCollapseAsR');
	if (stPn == 0) {
		pn.style.marginRight = '-360px';
		btn.style.transform = 'rotate(-180deg)';
		stPn = 1;
	}
	else {
		pn.style.marginRight = '0';
		btn.style.transform = 'rotate(0deg)';
		stPn = 0;
	}

}


//var runProgressbar = function (bar) {

//	var Porcentaje = 0.8;
//	bar.updateProgress(Porcentaje);

//	if (Porcentaje >= 1) {
//		App.pnAverage.hide();
//	}
//};


//function EsconderBarPgr(id) {
//	App.pnAverage.hide();
//}

//var stSldr = 0;
//var stSldrMbl = 0;
//function moveCtSldr(btn) {
//	let btnPrssd = btn.id;
//	let ct1 = document.getElementById('ctMain1');
//	let ct2 = document.getElementById('ctMain2');
//	let ct3 = document.getElementById('ctMain3');
//	var res = window.innerWidth;


//	if (res > 480) {


//		if (stSldr == 0 && btnPrssd == 'btnPrevSldr') {
//			App.ctMain2.hide();
//			App.btnPrevSldr.disable();
//			App.btnNextSldr.enable();

//			stSldr = 1;

//		}
//		else if (stSldr != 0 && btnPrssd == 'btnNextSldr') {
//			App.ctMain2.show();
//			App.btnPrevSldr.enable();
//			App.btnNextSldr.disable();
//			stSldr = 0;

//		}

//	}

//	else if (res <= 480) {
				
//			if (stSldrMbl == 0 && btnPrssd == 'btnPrevSldr') {
//				App.ctMain1.hide();
//				App.btnNextSldr.enable();
//				stSldrMbl = 1;

//			}

//			else if (stSldrMbl == 1 && btnPrssd == 'btnPrevSldr') {
//				App.ctMain2.hide();
//				App.btnPrevSldr.disable();
//				App.btnNextSldr.enable();
//				stSldrMbl = 2;

//			}

//			else if (stSldrMbl == 1 && btnPrssd == 'btnNextSldr') {
//				App.ctMain1.show();
//				App.btnPrevSldr.enable();
//				App.btnNextSldr.disable();
//				stSldrMbl = 0;

//			}

//			else if (stSldrMbl == 2 && btnPrssd == 'btnNextSldr') {
//				App.ctMain2.show();
//				App.btnPrevSldr.disable();
//				App.btnNextSldr.enable();
//				stSldrMbl = 1;

//			}
//	}

	
//}



//window.addEventListener('resize', function () {
//	var resol = window.innerWidth;
//	if (resol > 1024) {
//		App.ctMain2.show();
//		App.ctMain1.show();
//		App.btnPrevSldr.enable();
//		App.btnNextSldr.disable();
//		stSldr = 0;
//	}

//});


//BOTONES NAV

function habilitaLnk(vago) {
	let ct = document.getElementById('cntNavVistasForm-innerCt');
	let aLinks = ct.querySelectorAll('a');

	aLinks.forEach(function (itm) {
		itm.classList.remove("navActivo");
	});

	vago.classList.add('navActivo');
}


function showForms(who) {
	var LNo = who.textEl;


	switch (who.id) {
		case 'lnkSiteF':
			habilitaLnk(LNo);
			App.formSite.show();
			App.formLocation.hide();
			App.formAdditional.hide();
			break;

		case 'lnkLocationF':

			habilitaLnk(LNo);
			App.formLocation.show();
			App.formSite.hide();
			App.formAdditional.hide();
			break;

		case 'lnkAdditionalF':
			habilitaLnk(LNo);
			App.formAdditional.show();
			App.formLocation.hide();
			App.formSite.hide();
			break;

	}
}


