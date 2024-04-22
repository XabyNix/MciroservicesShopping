# Order Management System
Back end system in microservices architecture, that automatically manages orders payment and item reservation.

The system is composed of multiple services each with a specific scope, some of their expose REST endpoints, that can be integrated by a frontend application.

As a destributed system transactions is managed by a SAGA Choreography pattern as follow:
- POST checkout request arrives to `Cart service` and sends `CheckoutEvent`
- **Order service** listen for
  - `CheckoutEvent` and check the item availability with a grpc call to **catalogService**, if creates an order in the databse with a `pending` status ok then sends a `OrderCreated` event to the broker otherwise respond with an error.
  - `ItemReservationFailed` and delete the order from the database, returning a message to the caller.
  - `OrderConfirm` changes the state of the order to `confirmed`.
- **Catalog Service** listens for
  - `OrderCreated` and reserve items on another table in the database, and send a `ItemsReserved` event or `ItemReservationFailed` in case of exception.
  - `PaymentRejected` then  or execute a compensating transaction reservation and send `ItemReservationFailed` event.
  - `OrderConfirm` removes reserved items from the _Reservation_ table and removes the quantity from the main database.
- **Payment Service** listens for
  - `ItemsReserved` and procede to payment, if all good then sends a `OrderCOnfirm` event.


## Shema
![API Gateway](https://github.com/XabyNix/MciroservicesShopping/assets/25272328/6631f713-da9e-4aca-8b5e-c7ba4238f92f)

## Technology used
- **C#** with _.Net Core_ for writing service
- **Entity Framework** to making query and managing the database.
- **RabbitMQ** as queue message broker for events.
- **GRpc** to provide efficient synchronous comunication beetween the services over Http2.
