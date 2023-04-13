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


var stSldr = 0;
var stSldrMbl = 0;
function moveCtSldr(btn) {
	let btnPrssd = btn.id;
	let ct1 = document.getElementById('ctMain1');
	let ct2 = document.getElementById('ctMain2');
	let ct3 = document.getElementById('ctMain3');
	var res = window.innerWidth;


	if (res > 480) {


		if (stSldr == 0 && btnPrssd == 'btnPrevSldr') {
			App.ctMain2.hide();
			App.btnPrevSldr.disable();
			App.btnNextSldr.enable();

			stSldr = 1;

		}
		else if (stSldr != 0 && btnPrssd == 'btnNextSldr') {
			App.ctMain2.show();
			App.btnPrevSldr.enable();
			App.btnNextSldr.disable();
			stSldr = 0;

		}

	}

	else if (res <= 480) {

		if (stSldrMbl == 0 && btnPrssd == 'btnPrevSldr') {
			App.ctMain1.hide();
			App.btnNextSldr.enable();
			stSldrMbl = 1;

		}

		else if (stSldrMbl == 1 && btnPrssd == 'btnPrevSldr') {
			App.ctMain2.hide();
			App.btnPrevSldr.disable();
			App.btnNextSldr.enable();
			stSldrMbl = 2;

		}

		else if (stSldrMbl == 1 && btnPrssd == 'btnNextSldr') {
			App.ctMain1.show();
			App.btnPrevSldr.enable();
			App.btnNextSldr.disable();
			stSldrMbl = 0;

		}

		else if (stSldrMbl == 2 && btnPrssd == 'btnNextSldr') {
			App.ctMain2.show();
			App.btnPrevSldr.disable();
			App.btnNextSldr.enable();
			stSldrMbl = 1;

		}
	}


}



window.addEventListener('resize', function () {
	var resol = window.innerWidth;
	if (resol > 1024) {
		App.ctMain2.show();
		App.ctMain1.show();
		App.btnPrevSldr.enable();
		App.btnNextSldr.disable();
		stSldr = 0;
	}

});
