<script lang="ts">
  import Room from "../../components/Room.svelte";
  import { addRoom, getRooms, removeRoom } from "../../data-access/api/rooms-service";
  import WaitingComponent from "../../components/WaitingComponent.svelte";
  import { rooms } from "./rooms-store";
  import GuardComponent from "../../components/GuardComponent.svelte";
  import { executeWithLoadingAsync } from "../../data-access/loading-service";
  import auth from "../../data-access/auth-service";
  import { Button } from "flowbite-svelte";
  import NewRoom from "../../components/NewRoom.svelte";
  import BottomNavbar from "../../components/BottomNavbar.svelte";
  import type { RoomModel } from "../../models/room";
  
  let addnew = false;

  auth.subscribeWhenLogged(() => loadRooms());

  async function loadRooms() {
    await executeWithLoadingAsync(async () => {
      const result = await getRooms();
      rooms.set(result);
    });
  }

  async function saveRoom(name: string) {
    await addRoom(name);

    await loadRooms();
    removeNew();
  }

  async function deleteRoom(room: RoomModel) {
    // await removeRoom(room.id);

    rooms.update(currentRooms => {
      return currentRooms.filter(r => r.id != room.id);
    });
  }

  const roomUrl = (id: string) => `rooms/${id}`;
  const addNew = () => addnew = true;
  const removeNew = () => addnew = false;
</script>

<svelte:head>
	<title>iHome - Rooms</title>
	<meta name="description" content="iHome - Rooms" />
</svelte:head>

<GuardComponent>
  <WaitingComponent>
    {#each $rooms as room}
      <Room bind:room={room} urlGenerator={roomUrl} onRemove={deleteRoom} />
    {/each}

    {#if addnew}
      <NewRoom remove={removeNew} save={saveRoom} />
    {/if}
  </WaitingComponent>

  <BottomNavbar>
    <Button on:click={addNew} color="primary">Add Room</Button>
    <Button color="primary">Schedules</Button>
  </BottomNavbar>
</GuardComponent>