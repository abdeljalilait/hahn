export interface Ticket {
  id: number;
  description: string;
  status: "Open" | "Closed";
}

export interface TicketsResponse {
  data: Ticket[];
  pagination: {
    totalCount: number;
    pageSize: number;
    currentPage: number;
    totalPages: number;
  };
}
