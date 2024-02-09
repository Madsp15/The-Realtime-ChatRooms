export class BaseDto<T> {
  eventType: string;
 //uses the class name as the event type
  constructor(init?: Partial<T>) {
    this.eventType = this.constructor.name;
    Object.assign(this, init)
  }
}

export class ClientWantsToBroadCastToRoom extends BaseDto<ClientWantsToBroadCastToRoom> {
  Message?: string;
}
export class ClientWantsToSignIn extends BaseDto<ClientWantsToSignIn> {
  Username?: string;
}

export class ClientWantsToEnterRoom extends BaseDto<ClientWantsToEnterRoom> {
  RoomId?: number;
}
