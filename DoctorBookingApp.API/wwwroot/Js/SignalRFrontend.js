const connection = new signalR.HubConnectionBuilder()
    .withUrl("/bookingHub")
    .build();

connection.on("SlotBooked", function (slotId) {
    // Disable the slot in UI
    console.log("Slot booked:", slotId);
    disableSlotInUI(slotId);
});

connection.start().then(() => {
    connection.invoke("JoinDoctorGroup", doctorId);
});
