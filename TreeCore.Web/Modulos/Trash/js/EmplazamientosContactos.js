
// #region JS CONTROL RESIZER RESPONSIVE


setTimeout(function () {

	//REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
	GridResizer();
	App.grdContactos.update();

}, 100);

function GridResizer() {


	// #region CONTROL COLUMNA COLMORE MORE 


	const vw = Math.max(document.documentElement.clientWidth || 0, window.innerWidth || 0);

	if (vw < 1024 && vw > 480) {
		TreeCore.ColumnHider('Hide', '1024');
		App.grdContactos.update();
	}
	else if (vw < 480) {
		TreeCore.ColumnHider('Hide', '480');
		App.grdContactos.update();

	}
	else if (vw > 1024) {
		TreeCore.ColumnHider('Show', 'None');
		App.grdContactos.update();
	}

	// #endregion

}

// #endregion







var linkRendererTrack = function (value, metadata, record) {
	return Ext.String.format("<a href=\"javascript:addUserTabTrack('{0}','{1}');\">{1}<a/>", record.data.id, record.data.Code);
}


function addUserTabTrack(id, title) {
	var tabPanel = window.parent.App.tabPpal; //get from iframe  tabpanel located in parent window 
	var tab = tabPanel.getComponent('user_' + id);
	var url1 = '/PaginasComunes/Seguimiento.aspx';
	if (!tab) {
		tab = tabPanel.add({
			//id: 'user_' + id,
			title: title,
			iconCls: '#TextSignature',
			closable: true,
			//menuItem : menuItem,
			loader: {
				url: url1,
				renderer: "frame",
				loadMask: {
					showMask: true,
					msg: "Cargando Seguimiento..  "
					//msg: jsCargandoMapa
				}
			}
		});
	}

	tabPanel.setActiveTab(tab);
}

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



//Fin Ventana Modal Gestion Form

var stP = 0;
var stBtn = 0;

function hidePnR() {
	let pnR = document.getElementById('pnAsideR');
	pnR.style.width = '0';
}

function showPnAsideR(btnId) {
	if (stP == 0) {
		let pnR = document.getElementById('pnAsideR');
		pnR.style.width = '400px';
		App.pnAsideR.show();
		stP = 1;
	}


	switch (btnId) {

		case 'btnBuscar':
			App.btnFiltros.pressed = false;
			App.lblHeadSearch.show();
			App.pnSearch.show();

			App.lblHeadFilters.hide();
			App.pnCreateFltr.hide();
			App.pnMyFltr.hide();

			if (stBtn == 1) {
				hidePnR();
				stP = 0;
				stBtn = 0;
			}
			else {
				stBtn = 1;
			}
			break;

		case 'btnFiltros':
			App.btnBuscar.pressed = false;
			App.lblHeadFilters.show();
			App.pnCreateFltr.show();
			App.pnMyFltr.show();
			let ctn = document.getElementById('pnMyFltr_Content');
			ctn.style.display = 'block';;

			App.lblHeadSearch.hide();
			App.pnSearch.hide();

			if (stBtn == 2) {
				hidePnR();
				stP = 0;
				stBtn = 0;
			}
			else {
				stBtn = 2;
			}
			break;

	}
}



function ClickShowContractD() {

	App.WinContractDetails.show();

}

function ClickShowProjectsD() {

	App.WinProjectsDetails.show();

}


