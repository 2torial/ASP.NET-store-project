import { ProductCategory } from './StoreEnum/StoreProductCategory.ts'
import { PageSize } from './StoreEnum/StorePageSize.ts'
import { SortingMethod } from './StoreEnum/StoreSortingMethod.ts'
import { SortingOrder } from './StoreEnum/StoreSortingOrder.ts'

export interface InputChoosable {
    label: string,
    value: string
}

type ConvertableEnum = ProductCategory | PageSize | SortingMethod | SortingOrder; // Can be extended to accept more Enum types
type LabelMap<T extends ConvertableEnum> = Map<T, string>;

export const convertEnumToChoosable = <T extends ConvertableEnum>(enumValue: T, labelMap: LabelMap<T>): InputChoosable =>
    ({ label: labelMap.get(enumValue as T) as string, value: Object.keys(typeof enumValue)[enumValue as T] });