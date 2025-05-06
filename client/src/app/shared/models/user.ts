export type User = {
  firstName: string;
  lastName: string;
  email: string;
  address: Address;
};

export type Address = {
  line1: string;
  line2?: string;
  city: string;
  state: string;
  county: string;
  postalCode: string;
};
