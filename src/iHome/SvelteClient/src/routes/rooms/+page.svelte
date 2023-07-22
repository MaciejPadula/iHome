<script lang="ts">
  import { isAuthenticated, loading } from "../../data-access/store";
  import Room from "../../components/Room.svelte";
  import { onMount } from "svelte";
  import { getRooms } from "../../data-access/api/rooms-service";
  import WaitingComponent from "../../components/WaitingComponent.svelte";
  import { rooms } from "./rooms-store";
  import GuardComponent from "../../components/GuardComponent.svelte";

  isAuthenticated.subscribe(async authed => {
    if(!authed) return;

    await loadRooms();
  });

  async function loadRooms() {
    loading.set(true);
    const result = await getRooms();
    rooms.set(result);
    loading.set(false);
  }

  const roomUrl = (id: string) => `rooms/${id}`;
</script>

<GuardComponent>
  <WaitingComponent>
    {#each $rooms as room}
      <Room bind:room={room} urlGenerator={roomUrl} />
    {/each}
  </WaitingComponent>
</GuardComponent>