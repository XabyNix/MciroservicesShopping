// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/catalog.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace CatalogService {
  public static partial class Catalog
  {
    static readonly string __ServiceName = "itemDetail.Catalog";

    static readonly grpc::Marshaller<global::CatalogService.ItemRequest> __Marshaller_itemDetail_ItemRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::CatalogService.ItemRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::CatalogService.ItemResponse> __Marshaller_itemDetail_ItemResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::CatalogService.ItemResponse.Parser.ParseFrom);

    static readonly grpc::Method<global::CatalogService.ItemRequest, global::CatalogService.ItemResponse> __Method_GetItem = new grpc::Method<global::CatalogService.ItemRequest, global::CatalogService.ItemResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetItem",
        __Marshaller_itemDetail_ItemRequest,
        __Marshaller_itemDetail_ItemResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::CatalogService.CatalogReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Catalog</summary>
    [grpc::BindServiceMethod(typeof(Catalog), "BindService")]
    public abstract partial class CatalogBase
    {
      public virtual global::System.Threading.Tasks.Task<global::CatalogService.ItemResponse> GetItem(global::CatalogService.ItemRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(CatalogBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_GetItem, serviceImpl.GetItem).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, CatalogBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_GetItem, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::CatalogService.ItemRequest, global::CatalogService.ItemResponse>(serviceImpl.GetItem));
    }

  }
}
#endregion
