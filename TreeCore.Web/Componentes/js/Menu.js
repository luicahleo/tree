
function SelectItemMenu(sender, registro, index) {
	var seleccionado = registro.data;

	if (seleccionado != null &&
		seleccionado.RutaPagina != null && seleccionado.RutaPagina != "") {

		addTab(App.tabPpal, seleccionado.text, seleccionado.text, seleccionado.RutaPagina);
		this.getSelectionModel().deselectAll();
	}
}

var stMn = 0;
function collapsingMenu() {

	if (stMn == 0) {
		stMn = 1;
		displayMenu('close');
	}

	else {
		stMn = 0;
		displayMenu('open');
	}

}

var firstTm;
function displayMenu(action) {
	var logo = document.getElementById('cliente');
	var aside = document.querySelector('aside.aside');
	var centerCt = document.getElementById('centerDefault');
	var mn = document.querySelector('div.treeMenu');
	var elbows = document.querySelectorAll('div.x-tree-elbow-img');
	var elbowsN1 = document.querySelectorAll('[aria-level="2"] div.x-tree-elbow-img');
	var elbowsN3 = document.querySelectorAll('[aria-level="4"] div.x-tree-elbow-img:first-child');
	var spans = document.querySelectorAll('span.x-tree-node-text');
	var entornoLbl = document.getElementById('infoEntorno');
	var hr = document.querySelector('aside hr');



	switch (action) {

		case 'close':

			firstTm = 1;
			logo.style.opacity = '0';
			entornoLbl.style.visibility = 'hidden';
			hr.style.width = '40px';
			hr.style.marginLeft = '10px';
			mn.style.width = '65px';
			mn.classList.add('overNone');
			aside.style.width = '65px';
			aside.style.position = 'fixed';
			centerCt.style.marginLeft = '65px';
			centerCt.style.flexGrow = '1';


			for (i = 0; i < spans.length; i++) {
				spans[i].style.visibility = 'hidden';
			}

			for (i = 0; i < elbows.length; i++) {
				elbows[i].style.width = '0px';
			}

			for (i = 0; i < elbowsN1.length; i++) {
				elbowsN1[i].classList.add('elbowWider');
			}

			for (i = 0; i < elbowsN3.length; i++) {
				elbowsN3[i].style.marginRight = '0px';
			}

			break;

		case 'open':

			aside.style.width = '240px';
			aside.style.position = 'static';
			aside.style.height = '100%';
			aside.style.boxShadow = 'unset';
			centerCt.style.marginLeft = '0';
			centerCt.style.flexGrow = '1';
			mn.style.width = '100%';
			mn.classList.remove('overNone');
			mn.style.transition = 'width ease-out 0.3s';
			centerCt.style.position = 'static'
			centerCt.style.flexGrow = '1';


			for (i = 0; i < spans.length; i++) {
				spans[i].style.visibility = 'visible';
			}

			for (i = 0; i < elbows.length; i++) {
				elbows[i].style.width = '18px';
			}

			for (i = 0; i < elbowsN1.length; i++) {
				elbowsN1[i].classList.remove('elbowWider');
			}

			for (i = 0; i < elbowsN3.length; i++) {
				elbowsN3[i].style.marginRight = '4px';
			}

			hr.style.width = '70%';
			hr.style.marginLeft = 'unset';
			hr.style.margin = '10px auto';
			logo.style.opacity = '100%';
			entornoLbl.style.visibility = 'visible';

			firstTm = 0;
			break;

		case 'over':

			if (stMn == 1 && firstTm == 0) {


				aside.style.width = '240px';
				aside.style.zIndex = '999';
				aside.style.height = '100%';
				aside.style.boxShadow = '2px 3px 5px rgba(0,0,0,0.14)';


				mn.style.width = '100%';
				mn.classList.remove('overNone');
				mn.style.transition = 'width ease-out 0.3s';
				for (i = 0; i < spans.length; i++) {
					spans[i].style.visibility = 'visible';
				}

				for (i = 0; i < elbows.length; i++) {
					elbows[i].style.width = '18px';
				}

				for (i = 0; i < elbowsN1.length; i++) {
					elbowsN1[i].classList.remove('elbowWider');
				}

				for (i = 0; i < elbowsN3.length; i++) {
					elbowsN3[i].style.marginRight = '4px';
				}

				hr.style.width = '70%';
				hr.style.marginLeft = 'unset';
				hr.style.margin = '10px auto';
				
				
				let perc = aside.clientWidth;
				//for (i = 100; i < perc; i++) {

				//	if (perc >= 140) {
				//		logo.style.opacity = (perc -140) +'%';
				//	}
					
				//}
				logo.style.opacity = 1;

				entornoLbl.style.visibility = 'visible';

				
			}
			break;

		case 'out':

			if (stMn == 1) {
				aside.style.height = '100%';
				aside.style.boxShadow = 'unset';


				logo.style.opacity = 0;
				entornoLbl.style.visibility = 'hidden';
				hr.style.width = '40px';
				hr.style.marginLeft = '12px';
				mn.style.width = '65px';
				mn.classList.add('overNone');
				aside.style.width = '65px';


				for (i = 0; i < spans.length; i++) {
					spans[i].style.visibility = 'hidden';
				}

				for (i = 0; i < elbows.length; i++) {
					elbows[i].style.width = '0px';
				}

				for (i = 0; i < elbowsN1.length; i++) {
					elbowsN1[i].classList.add('elbowWider');
				}

				for (i = 0; i < elbowsN3.length; i++) {
					elbowsN3[i].style.marginRight = '0px';
				}

			}

			firstTm = 0;
			break;

		default:
			firstTm = 0;
			break;

	}
}

function resizeMenu(w, h) {
	setTimeout(function () {
		App.ComponenteMenu_Tree.setHeight(0);
		let elementoArbol = App.ComponenteMenu_Tree.ariaEl.getParent();
		let padre = elementoArbol.getParent();
		let altura;

		
		if (padre.dom.scrollHeight >= 900) {
			altura = (padre.dom.scrollHeight * 0.75) - 128;
		}

		else if (padre.dom.scrollHeight >= 650 && padre.dom.scrollHeight < 900) {
			altura = (padre.dom.scrollHeight * 0.70) - 128;
		}

		else if (padre.dom.scrollHeight >= 300 && padre.dom.scrollHeight < 650) {
			altura = (padre.dom.scrollHeight * 0.6) - 128;
		}

		else {
			altura = (padre.dom.scrollHeight * 0.5);
		}

		if (padre.dom.parentElement.clientWidth <= 620) {
			stMn = 1;
			displayMenu('close');
		}
		else {
			stMn = 0;
			displayMenu('open');
        }

		App.ComponenteMenu_Tree.setHeight(altura);

	}, 100);
}

Ext.EventManager.onWindowResize(function (w, h) {
    resizeMenu(w, h);
});

