// Establecer la conexi�n de SignalR
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

// Configuraci�n de toastr
toastr.options = {
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut",
};

connection.on("ReceiveNotification", function (message) {
    // Aseg�rate de que solo se muestre una notificaci�n
    if (!window.notificationDisplayed) {
        window.notificationDisplayed = true;
        setTimeout(() => window.notificationDisplayed = false, 60000);

        // Mostrar la notificaci�n
        toastr.info(message); 
    }
});

// Iniciar la conexi�n
connection.start().catch(function (err) {
    console.error('SignalR error: ', err.toString());
});
