// Establecer la conexión de SignalR
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

// Configuración de toastr
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
    // Asegúrate de que solo se muestre una notificación
    if (!window.notificationDisplayed) {
        window.notificationDisplayed = true;
        setTimeout(() => window.notificationDisplayed = false, 60000);

        // Mostrar la notificación
        toastr.info(message); 
    }
});

// Iniciar la conexión
connection.start().catch(function (err) {
    console.error('SignalR error: ', err.toString());
});
