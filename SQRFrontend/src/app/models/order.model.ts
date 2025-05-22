import { Material } from "./material.model";

export interface Order {
  order: string;
  quantity: number;
  productCode: string;
  productDescription: string;
  image: string;
  cycleTime: number;
  materials: Material[];
}

