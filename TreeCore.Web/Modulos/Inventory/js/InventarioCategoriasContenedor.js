function NavegacionTabs(sender) {
    var senderid = sender.id;
    if (senderid == 'lnkCategorias') {
        App.ctMain1.show();
        App.ctMain2.hide();
        App.ctMain3.hide();
        App.ctMain4.hide();
        //App.ctMain5.hide();
    }
    else if (senderid == 'lnkVinculaciones') {
        App.ctMain1.hide();
        App.ctMain2.show();
        App.ctMain3.hide();
        App.ctMain4.hide();
        //App.ctMain5.hide();
    }
    else if (senderid == 'lnkSubcategorias') {
        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.hide();
        App.ctMain4.show();
        //App.ctMain5.hide();
    }
    else if (senderid == 'lnkDiagramas') {
        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.show();
        App.ctMain4.hide();
        //App.ctMain5.hide();
    }
    else {
        App.ctMain1.show();
        App.ctMain2.hide();
        App.ctMain3.hide();
        App.ctMain4.hide();
        //App.ctMain5.hide();
    }

    document.getElementById('lnkCategorias').classList.remove("navActivo");
    document.getElementById('lnkCategorias').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkVinculaciones').classList.remove("navActivo");
    document.getElementById('lnkVinculaciones').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkSubcategorias').classList.remove("navActivo");
    document.getElementById('lnkSubcategorias').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkDiagramas').classList.remove("navActivo");
    document.getElementById('lnkDiagramas').childNodes[1].classList.remove("navActivo");
    document.getElementById(senderid).classList.add("navActivo");
    document.getElementById(senderid).childNodes[1].classList.add("navActivo");

    /*  if (App.ctMain1.hidden && App.ctMain2.hidden) {
          App.ctMain1.show();
          App.ctMain2.hide();
      } else if (App.ctMain1.hidden && App.ctMain2.hidden) {
          App.ctMain1.hide();
          App.ctMain2.show();
      }
      else if (App.ctMain1.hidden && App.ctMain2.hidden) {
  
      }*/
}