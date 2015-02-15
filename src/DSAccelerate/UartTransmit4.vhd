----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    19:40:36 02/09/2015 
-- Design Name: 
-- Module Name:    UartTransmit4 - Behavioral 
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
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx primitives in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity UartTransmit4 is
	Port( 
			clock : in STD_LOGIC;
			reset : in STD_LOGIC;
			start_transmit_4bytes: in STD_LOGIC;
			data_4bytes : in STD_LOGIC_VECTOR(31 downto 0);
			send_4bytes_complete : out STD_LOGIC;
			RS232_dataout : out STD_LOGIC
		 );
end UartTransmit4;

architecture Behavioral of UartTransmit4 is

	type TRANSMIT_4BYTE_STATE is( 
						 STATE_IDLE, 
						 STATE_TRANSMIT,
						 STATE_DONE
					  );
					  
	component UART is
		Port( 
				clock  : in  STD_LOGIC;
				reset : in  STD_LOGIC;	
				start_transmit : in STD_LOGIC;
				send_byte : in STD_LOGIC_VECTOR(7 downto 0);
				dataout : out STD_LOGIC;
				byte_sent : out STD_LOGIC 
			 );
	end component UART;

	signal baudRateEnable : STD_LOGIC := '0';
	signal transmit_byte_done : STD_LOGIC := '0';
	signal start_send_byte : STD_LOGIC := '0';
	signal byte_to_send : STD_LOGIC_VECTOR(7 downto 0);
	signal transmit_state : TRANSMIT_4BYTE_STATE := STATE_IDLE;

begin
	
	uart_transmitdata : UART port map
	(
		clock => clock,
		reset => reset,
		start_transmit => start_send_byte,
		send_byte => byte_to_send,
		dataout => RS232_dataout,
		byte_sent => transmit_byte_done
	);

	tx_Queue_Bytes : process(clock, reset)
		variable lo_bits : integer;
		variable hi_bits : integer;
	begin
		ResetSync : if reset = '1' then
			lo_bits := 0;
			hi_bits := 7;
			start_send_byte <= '0';
			send_4bytes_complete <= '0';
			transmit_state <= STATE_IDLE;
		elsif reset = '0' then 
			Clocksync : if rising_edge(clock) then
				send_4bytes_complete <= '0';
				sm_transmit : if transmit_state = STATE_IDLE then 
					if start_transmit_4bytes = '1' then
						transmit_state <= STATE_TRANSMIT;
					end if;
				elsif transmit_state = STATE_TRANSMIT then 
					CanSendByte : if start_send_byte = '0' then
						if lo_bits = 32 then
							-- Reset 
							lo_bits := 0;
							hi_bits := 7;
							send_4bytes_complete <= '1';
							transmit_state <= STATE_DONE;
						else
							start_send_byte <= '1';
							byte_to_send(7 downto 0) <= data_4bytes(hi_bits downto lo_bits);
							lo_bits := lo_bits + 8;
							hi_bits := hi_bits + 8;
						end if;
					elsif transmit_byte_done = '1' then
						-- Last byte sent, set state for sending next byte
						start_send_byte <= '0';
					end if CanSendByte;
					
				elsif transmit_state = STATE_DONE then 
					transmit_state <= STATE_IDLE;
				end if sm_transmit;
			end if Clocksync;
		end if ResetSync;
	end process tx_Queue_Bytes;
end Behavioral;

