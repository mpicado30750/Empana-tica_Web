
//// Objeto para manejar el loader global
//const LoaderManager = {
//    show: function (message = 'Cargando...') {
//        const loader = document.getElementById('globalLoader');
//        const loadingText = loader.querySelector('.loading-text');
//        loadingText.textContent = message;
//        loader.style.display = 'flex';
//        document.body.style.overflow = 'hidden'; // Previene el scroll
//    },

//    hide: function () {
//        const loader = document.getElementById('globalLoader');
//        loader.style.display = 'none';
//        document.body.style.overflow = ''; // Restaura el scroll
//    }
//};

//// Para poder usar el loader en jQuery ajax
//$(document).ajaxStart(function () {
//    LoaderManager.show();
//}).ajaxStop(function () {
//    LoaderManager.hide();
//});
