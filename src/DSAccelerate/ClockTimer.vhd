----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    22:21:10 02/07/2015 
-- Design Name: 
-- Module Name:    ClockTimer - Behavioral 
-- Project Name: 
-- Target Devices: 
-- Tool versions: 
-- Description: 
--
-- Dependencies: 
--
-- Revision: 
-- Revision 0.01 - File Created
-- Additional Comments: 
--
----------------------------------------------------------------------------------
library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx primitives in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity ClockTimer is
	Port( 
			clock : in STD_LOGIC;
			reset : in STD_LOGIC;
			start : in STD_LOGIC;
			done : in STD_LOGIC;
			clockticks_elapsed : out STD_LOGIC_VECTOR(63 downto 0);
			ms_elapsed : out STD_LOGIC_VECTOR(31 downto 0)
		 );
end ClockTimer;

architecture Behavioral of ClockTimer is
	
	-- 1ms in clock cycles
	-- 50mhz / 1000 
	constant ms_50mhz_clock_cycles : integer := 50000;

begin

	timer : process(clock, reset)
		variable iMsElapsed : integer := 0;		-- milliseconds elapsed since execution started
		variable clk_elapsed : integer := 0;	-- clock timer that resets for every millisecond
		variable iClockCyclesElapsed : integer := 0;	-- clock ticks since the execution started
	
	begin
		ResetSync : if reset = '1' then
			iMsElapsed := 0;
			clk_elapsed := 0;
			
		elsif reset = '0' then
			ClockSync : if rising_edge(clock) then
				StartTimer : if start = '1' then
					if done = '0' then
						iClockCyclesElapsed := iClockCyclesElapsed + 1;
						IsOneMillisecond : if clk_elapsed = ms_50mhz_clock_cycles then
							clk_elapsed := 0;
							iMsElapsed := iMsElapsed + 1;
						else
							clk_elapsed := clk_elapsed + 1;
						end if IsOneMillisecond;
					elsif done = '1' then
						ms_elapsed <= std_logic_vector(to_unsigned(iMsElapsed, 32));
						clockticks_elapsed <= std_logic_vector(to_unsigned(iClockCyclesElapsed, 64));
					end if;
				end if StartTimer; 
			end if ClockSync;
		end if ResetSync;
	end process timer;
end Behavioral;

