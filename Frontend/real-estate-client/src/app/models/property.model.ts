import { PropertyTraceDto } from "./PropertyTrace.model";

export interface PropertyDto {
  idProperty?: number;
  name: string;
  address: string;
  price: number;
  codeInternal: string;
  year: number;
  idOwner: number;
  image?: string;
  traces: PropertyTraceDto[];
}