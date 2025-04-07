import { ProductCategory } from './StoreEnum/StoreProductCategory.ts'
import { PageSize } from './StoreEnum/StorePageSize.ts'
import { SortingMethod } from './StoreEnum/StoreSortingMethod.ts'
import { SortingOrder } from './StoreEnum/StoreSortingOrder.ts'

export interface InputChoosable {
    label: string,
    value: string
}

type ConvertableEnum = ProductCategory | PageSize | SortingMethod | SortingOrder; // Can be extended to accept more Enum types

export const convertEnumToChoosable = (enumValue: ConvertableEnum, labelMap: Map<ConvertableEnum, string>): InputChoosable =>
    ({ label: labelMap.get(enumValue as ConvertableEnum) as string, value: enumValue.toString() });