const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

toastr.options = {
    "closeButton": true,
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
    "hideMethod": "fadeOut"
};

// Escucha el evento de notificaci�n
connection.on("ReceiveNotification", function (message) {
    toastr.success(message, 'Notificaci�n');
});

connection.start().catch(function (err) {
    console.error('SignalR error: ', err.toString());
});
