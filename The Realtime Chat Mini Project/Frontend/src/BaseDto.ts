export class BaseDto<T> {
  eventType: string;
 //uses the class name as the event type
  constructor(init?: Partial<T>) {
    this.eventType = this.constructor.name;
    Object.assign(this, init)
  }
}

export class ClientWantsToBroadCastToRoom extends BaseDto<ClientWantsToBroadCastToRoom> {
  message?: string;
  roomId?: number;
}

export class ClientWantsToBroadCastToRooms extends BaseDto<ClientWantsToBroadCastToRoom> {
  message?: string;
}
export class ClientWantsToSignIn extends BaseDto<ClientWantsToSignIn> {
  username?: string;
}

export class ClientWantsToEnterRoom extends BaseDto<ClientWantsToEnterRoom> {
  roomId?: number;
}
export class ReturnRoomsUserIsIn extends BaseDto<ReturnRoomsUserIsIn> {
}
