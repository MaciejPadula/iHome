<script lang="ts">
  import { Avatar, DarkMode, Dropdown, DropdownDivider, DropdownHeader, DropdownItem } from "flowbite-svelte";
  import { isAuthenticated, user } from "../data-access/store";
  import auth from "../data-access/auth-service";

  async function login() {
		await auth.login();
	}

  async function logout() {
    await auth.logout();
  }

</script>


{#if $isAuthenticated == true}
  <div id="login-menu" class="flex items-center md:order-2 cursor-pointer">
    <Avatar src={$user?.picture} />
  </div>
  <Dropdown placement="bottom" triggeredBy="#login-menu">
    <DropdownHeader class="flex">
      <div class="mr-2">
        <p>{$user?.name}</p>
        <span class="block truncate text-sm font-medium"> {$user?.email} </span>
      </div>
      <DarkMode />
    </DropdownHeader>
    <DropdownItem href="/rooms">Rooms</DropdownItem>
    <DropdownItem href="/schedules">Schedules</DropdownItem>
    <DropdownDivider />
    <DropdownItem on:click={logout}>Sign out</DropdownItem>
  </Dropdown>
{/if}

{#if $isAuthenticated == false}
  <div class="flex items-center md:order-2 h-10">
    <button on:click={login}>Login</button>
  </div>
{/if}