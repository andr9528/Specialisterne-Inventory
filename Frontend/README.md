## Project Structure
The following structure is a baseline example. Should be updated when the project is under development.

src/
    app/
        App.tsx
        routes.tsx
        providers.tsx
        context/


    features/
        products/
            components/
                ProductTable.tsx
                ProductRow.tsx
                ProductFilters.tsx

            hooks/
                useProducts.ts
                useCreateProduct.ts

            services/
                product.service.ts

            types/
                product.types.ts

            pages/
                ProductsPage.tsx

        orders/
            components/
                OrderTable.tsx
                OrderRow.tsx

            hooks/
                useOrders.ts
                useCreateOrder.ts

            services/
                order.service.ts

            types/
                order.types.ts

            pages/
                OrdersPage.tsx

    shared/
        components/

        hooks/

        utils/

        types/
