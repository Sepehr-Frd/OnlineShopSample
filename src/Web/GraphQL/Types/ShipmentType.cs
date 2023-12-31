using Application.EntityManagement.Addresses.Queries;
using Application.EntityManagement.Orders.Queries;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Web.GraphQL.Types;

public class ShipmentType : ObjectType<Shipment>
{
    protected override void Configure(IObjectTypeDescriptor<Shipment> descriptor)
    {
        descriptor
            .Description("Represents a shipment with details like status, shipping method, dates, cost, and associated addresses.");

        descriptor
            .Field(shipment => shipment.Order)
            .ResolveWith<Resolvers>(
                resolvers =>
                    Resolvers.GetOrderAsync(default!, default!))
            .Description("The Order Associated with the Shipment\n" +
                         "Authentication is required.");

        descriptor
            .Field(shipment => shipment.DestinationAddressId)
            .ResolveWith<Resolvers>(
                resolvers =>
                    Resolvers.GetDestinationAddressAsync(default!, default!))
            .Description("The Destination Address Associated with the Shipment\n" +
                         "Authentication is required.");

        descriptor
            .Field(shipment => shipment.OriginAddressId)
            .ResolveWith<Resolvers>(
                resolvers =>
                    Resolvers.GetOriginAddressAsync(default!, default!))
            .Description("The Origin Address Associated with the Shipment\n" +
                         "Authentication is required.");

        descriptor
            .Field(shipment => shipment.DateCreated)
            .Description("The Creation Date");

        descriptor
            .Field(shipment => shipment.DateModified)
            .Description("The Last Modification Date");

        descriptor
            .Field(shipment => shipment.ExternalId)
            .Description("The External ID for Client Interactions");

        descriptor
            .Field(shipment => shipment.InternalId)
            .Ignore();

        descriptor
            .Field(shipment => shipment.OrderId)
            .Ignore();

        descriptor
            .Field(shipment => shipment.DestinationAddressId)
            .Ignore();

        descriptor
            .Field(shipment => shipment.OriginAddressId)
            .Ignore();
    }

    private sealed class Resolvers
    {
        public static async Task<Order?> GetOrderAsync([Parent] Shipment shipment, [Service] ISender sender)
        {
            var ordersQuery = new GetAllOrdersQuery(
                new Pagination(),
                x => x.InternalId == shipment.OrderId);

            var result = await sender.Send(ordersQuery);

            return result.Data?.FirstOrDefault();
        }

        public static async Task<Address?> GetDestinationAddressAsync([Parent] Shipment shipment, [Service] ISender sender)
        {
            var addressesQuery = new GetAllAddressesQuery(
                new Pagination(),
                x => x.InternalId == shipment.DestinationAddressId);

            var result = await sender.Send(addressesQuery);

            return result.Data?.FirstOrDefault();
        }

        public static async Task<Address?> GetOriginAddressAsync([Parent] Shipment shipment, [Service] ISender sender)
        {
            var addressesQuery = new GetAllAddressesQuery(
                new Pagination(),
                x => x.InternalId == shipment.OriginAddressId);

            var result = await sender.Send(addressesQuery);

            return result.Data?.FirstOrDefault();
        }
    }
}