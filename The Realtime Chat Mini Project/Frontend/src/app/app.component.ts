import { Component } from '@angular/core';
import {FormControl} from "@angular/forms";
import {
  BaseDto,
  ClientWantsToBroadCastToRoom, ClientWantsToBroadCastToRooms,
  ClientWantsToEnterRoom,
  ClientWantsToSignIn,
  ReturnRoomsUserIsIn
} from "../BaseDto";
import {Message, ServerAddsClientToRoom} from "../ServerAddsClientToRoom";


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})


export class AppComponent {
  title = 'Chatrooms';
  messages: Message[] = [];

  ws: WebSocket = new WebSocket('ws://localhost:8181');
  messageContent = new FormControl('');
  SelectedUserName = new FormControl('');
  SelectedRoomNumber  = new FormControl('');
  SentMessage= new FormControl('');
  constructor() {
    this.ws.onmessage = (event) => {
        this.breakUpResponse(event.data)
    }
  }

  private breakUpResponse(response: string) {
    let messageObj = JSON.parse(response) as ServerAddsClientToRoom;
    if (response.includes("ServerAddsClientToRoom")) {
      let add1 = messageObj.lastMessages!;
      this.messages = this.messages.concat(add1);
    }
    this.messages.push(messageObj);

  }


  sendMessage() {
    var num = parseInt(this.SelectedRoomNumber.value!);
    let o = new ClientWantsToBroadCastToRoom({message: this.messageContent.value?.toString(),roomId : num})
    this.ws.send(JSON.stringify(o));
  }

  sendMessages() {
    let o = new ClientWantsToBroadCastToRooms({message: this.messageContent.value?.toString()})
    this.ws.send(JSON.stringify(o));
  }
  signInUser() {
   let o = new ClientWantsToSignIn({username: this.SelectedUserName.value?.toString()})
      this.ws.send(JSON.stringify(o));
  }

  selectRoom() {
   var num = parseInt(this.SelectedRoomNumber.value!);
   let x = new ClientWantsToEnterRoom({roomId: num })
    this.ws.send(JSON.stringify(x));
  }
  ReturnRooms() {
    let o = new ReturnRoomsUserIsIn()
    this.ws.send(JSON.stringify(o));
  }
  protected readonly BaseDto = BaseDto;
}


